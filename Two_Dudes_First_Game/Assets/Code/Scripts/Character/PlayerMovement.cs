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
    private float jumpingPower = 16f;
    private Inventory inventory;
    public bool isFacingRight = true;

    public static PlayerMovement Instance { get; private set; }

    public bool attack = false;
    private float lastY;
    public bool Falling = false;
    public bool FreeFall = false;
    public bool jump = false;
    public float FallingThreshold = -0.01f;  //Adjust in inspector to appropriate value for the speed you want to trigger detecting a fall, probably by just testing (use negative numbers probably)
    float horizontalMove = 0f;

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

    void Start()
    {
        lastY = transform.position.y;

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            //Touching item
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    void Update()
    {

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
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jump = true;
            animator.SetBool("Jump", true);
        }

        // Allows the release of jump for shorter or longer jumps
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // Salt Attack / Set Animation
        if (Input.GetButtonDown("Fire1"))
        {
            // Instantiate salt prefab at the launch offset position, facing charaters direction
            Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            animator.SetBool("Attack", true);
            //animator.SetBool("Attack", false);
        }

        //  Reset Salt Attack Animation 
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("Attack", false);
        }

        // sets public state when player is fall/freefalling
        DetectFallandFreeFall();

        // sets animation for player based on falling state
        SetFallAnimations();


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
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public void onLanding()
    {
        animator.SetBool("Jump", false);
    }

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