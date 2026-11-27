using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 5f;
    private float FallTime = 0;
    [SerializeField] private float jumpingPower = 14f; // ~2.5 units high; the WorldMap tree steps need 2.2
    private Inventory inventory;
    public bool isFacingRight = true;

    public static PlayerMovement Instance { get; private set; }

    public bool attack = false;
    private float lastY;
    public float lastX;
    public bool Falling = false;
    public bool FreeFall = false;
    public bool jump = false;
    public float FallingThreshold = -0.01f;  //Adjust in inspector to appropriate value for the speed you want to trigger detecting a fall, probably by just testing (use negative numbers probably)
    float horizontalMove = 0f;
    private float knockbackTimer = 0f;
    private float airTime;

    // set by cutscenes like Frank waking up
    public static bool Locked;
    private float stepTimer;
    private float throwTimer;
    [SerializeField] private float throwPose = 0.2f;
    [SerializeField] private float throwCooldown = 0.3f;

    //public CharacterController controller;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private UI_Inventory uiInventory;


    // Projectile
    public SaltBehaviour ProjectilePrefab;
    public Transform LaunchOffset;

    void Awake() // inventory functions
    {
        Instance = this;

        //ItemWorld.SpawnItemWorld(new Vector3(3, -8), new Item { itemType = Item.ItemType.BasementKey, amount = 1 });

    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    void Start()
    {
        lastY = transform.position.y;
        lastX = transform.position.x;

        inventory = new Inventory();
        inventory.LoadOwned();
        if (uiInventory != null) uiInventory.SetInventory(inventory);

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            //Touching item
            inventory.AddItem(itemWorld.GetItem());
            Sound.Play("keyPickup");
            itemWorld.DestroySelf();
        }
    }

    void Update()
    {
        if (PauseMenu.IsPaused)
        {
            return;
        }

        if (Locked)
        {
            horizontal = 0f;
            animator.SetFloat("Speed", 0f);
            return;
        }

        // this will pause player movement if bathroom dialogue window is open
        if (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        lastY = transform.position.y;
        lastX = transform.position.x;
        horizontal = Input.GetAxisRaw("Horizontal");
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        // set animator speed so animation starts running animation
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        // when player in grounded or removes jump input animation will stop
        if (Input.GetButtonUp("Jump")  || Falling || IsGrounded())
        {
            animator.SetBool("Jump", false);

        }

        // Jump if player is on the ground, set by IsGrounded()
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            jump = true;
            Sound.Play("jump");
            animator.SetBool("Jump", true);
        }

        // Allows the release of jump for shorter or longer jumps
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // salt throw: the throw pose holds long enough to see even on a tap, with a short cooldown
        throwTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && throwTimer <= -throwCooldown + throwPose)
        {
            SaltBehaviour salt = Instantiate(ProjectilePrefab, LaunchOffset.position, Quaternion.identity);
            salt.facingRight = isFacingRight;
            Sound.Play("saltThrow");
            animator.SetBool("Attack", true);
            // first throw after filling up on holy water
            if (PlayerPrefs.GetInt("holyWater") == 1 && PlayerPrefs.GetInt("blessedSaltNoticed") == 0)
            {
                PlayerPrefs.SetInt("blessedSaltNoticed", 1);
                PlayerBark.Say("Huh... my salt looks different.");
            }
            throwTimer = throwPose;
        }
        if (throwTimer <= 0f && animator.GetBool("Attack"))
        {
            animator.SetBool("Attack", false);
        }

        // sets public state when player is fall/freefalling
        DetectFallandFreeFall();

        // sets animation for player based on falling state
        SetFallAnimations();

        Footsteps();


        //flips player from left to right
        Flip();
    }

    // detect falling and set state to match fall/freefall
    private void DetectFallandFreeFall()
    {
        float distancePerSecondSinceLastFrame = (transform.position.y - lastY) * Time.deltaTime;
        lastY = transform.position.y;  //set for next frame
        if (distancePerSecondSinceLastFrame < FallingThreshold)
        {
            Falling = true;
            FallTime = FallTime += 1;
        }
        else
        {
            Falling = false;
            FallTime = 0;
        }

        if (FallTime >= 4)
        {
            Debug.Log("FREEFALLING");
            FreeFall = true;
        }

        else if (IsGrounded() || !Falling)
        {
            FreeFall = false;
            FallTime = 0;
        }
    }

    // sets animation for player based on falling state
    private void SetFallAnimations()
    {

        if (Falling)
        {
            animator.SetBool("Jump", false);
        }

        if (FreeFall)
        {
            animator.SetBool("Falling", true);
        }

        if (!FreeFall || IsGrounded())
        {
            animator.SetBool("Falling", false);
        }

        if (IsGrounded())
        {
            animator.SetBool("Falling", false);
        }
    }

    private void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        rb.linearVelocity = velocity;
        knockbackTimer = duration;
    }

    public void onLanding()
    {
        animator.SetBool("Jump", false);
    }

    private void Footsteps()
    {
        if (!IsGrounded())
        {
            airTime += Time.deltaTime;
            return;
        }
        if (airTime > 0.25f)
        {
            Sound.Play("land");
            stepTimer = 0.2f;
        }
        airTime = 0f;
        if (Mathf.Abs(horizontal) > 0.1f && knockbackTimer <= 0f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                Sound.Play("step");
                stepTimer = 0.3f;
            }
        }
        else
        {
            stepTimer = 0.05f;
        }
    }

    public bool Grounded => IsGrounded();

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // flips player left and right based on 
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}