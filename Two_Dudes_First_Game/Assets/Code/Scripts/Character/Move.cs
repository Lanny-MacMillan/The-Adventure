using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;
//using Transform = UnityEngine.Transform;

//public class Move : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public float _moveSpeed = 8f;
//    public float _horizontal;
//    public float _jumpPower = 16f;
//    public bool _isFacingRight = true;

//    public const string RIGHT = "right";
//    public const string LEFT = "left";

//    string buttonPressed;

//    [SerializeField] private Rigidbody2D rb;
//    [SerializeField] private Transform groundCheck;
//    [SerializeField] private LayerMask groundLayer;

//    void Start()
//    {
//        _horizontal = Input.GetAxisRaw("Horizontal");
//        if (Input.GetButtonDown("Jump") && IsGrounded())
//        {
//            rb.velocity = new Vector2(rb.velocity.x, _jumpPower);
//        }
//        if (Input.GetButtonUp("Jump") && IsGrounded())
//        {
//            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
//        }
//        Flip(); 
//        //rd2d = GetComponent<Rigidbody2D>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //if ( Input.GetKey(KeyCode.RightArrow ))
//        //{
//        //    buttonPressed = RIGHT;
//        //    transform.Translate(Vector2.right * (_moveSpeed * Time.deltaTime), Space.World);

//        //    //telling position to transform right
//        //    // another way to do it
//        //    //transform.position += transform.right * (_moveSpeed * Time.deltaTime);
//        //}
//        //else if ( Input.GetKey(KeyCode.LeftArrow ))
//        //{
//        //    buttonPressed = LEFT;
//        //    transform.Translate(Vector2.left * (_moveSpeed * Time.deltaTime), Space.World);

//        //    //telling position to transform left
//        //    // another way to do it
//        //    //transform.position -= transform.right * (_moveSpeed * Time.deltaTime);

//        //}
//        //else
//        //{
//        //    buttonPressed = null;
//        //}
//    }

//    private void FixedUpdate()
//    {
//        rb.velocity = new Vector2(_horizontal * _moveSpeed, rb.velocity.y);

//        //if (buttonPressed == RIGHT)
//        //{

//        //}
//        //else if (buttonPressed == LEFT)
//        //{

//        //}
//        //else
//        //{
//        //    new Vector2(0, 0);
//        //}
//    }
//    private bool IsGrounded()
//    {
//        // creates and invisible circle at plarchar feet to detect floor collisions
//        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
//    }
//    private void Flip()
//    {
//        if(_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
//        {
//            _isFacingRight = !_isFacingRight;
//            Vector3 localScale = transform.localScale;
//            localScale.x *= -1f;
//            transform.localScale = localScale;
//        }
//    }
//}

