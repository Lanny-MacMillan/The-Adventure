using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float FallTime = 0;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    //private bool grounded = false;
    private float lastY;
    public bool Falling = false;
    public bool FreeFall = false;
    public bool jump = false;
    public float FallingThreshold = -0.01f;  //Adjust in inspector to appropriate value for the speed you want to trigger detecting a fall, probably by just testing (use negative numbers probably)

    //public CharacterController controller;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    float horizontalMove = 0f;

    void Start()
    {
        lastY = transform.position.y;
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



        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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

        if (FallTime == 4)
        {
            Debug.Log("FREEFALLING");
            FreeFall = true;
        }
        else if (IsGrounded() || !Falling)
        {
            //Debug.Log("GROUNDED");

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