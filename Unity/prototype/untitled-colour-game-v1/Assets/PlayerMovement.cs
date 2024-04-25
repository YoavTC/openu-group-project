using System;
using NaughtyAttributes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask wallGroundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float decelerationRate, maxSpeed;
    private float moveInput;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [ReadOnly] [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Wall Jumps")] 
    [SerializeField] private float wallJumpForce;
    [ReadOnly] [SerializeField] private bool mountedRightWall, mountedLeftWall, isWallJumping;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Horizontal movement
        if (transform.position.x < 56.8f)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else moveInput = 0;
       
        
        //Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        //Wall jumping
        if (!isGrounded && Input.GetKeyDown(KeyCode.Space) && (mountedRightWall || mountedLeftWall))
        {
            Jump();
            rb.velocity = Vector2.zero;
            isWallJumping = true;
            
            if (mountedRightWall)
            {
                Debug.Log("Wall jumping left");
                rb.velocity = new Vector2(-1 * wallJumpForce, 0);
            }
            if (mountedLeftWall)
            {
                Debug.Log("Wall jumping right");
                rb.velocity = new Vector2(wallJumpForce, 0);
            }
        }

        if (transform.position.y < -7) Respawn();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallGroundLayer);
        
        Vector3 transformPosition = transform.position;
        mountedRightWall =
            Physics2D.OverlapPoint(transformPosition + new Vector3(0.25f, 0f, 0f), wallGroundLayer);
        
        mountedLeftWall =
            Physics2D.OverlapPoint(transformPosition + new Vector3(-0.25f, 0f, 0f), wallGroundLayer);

        if (isGrounded) isWallJumping = false;
        //if (!isGrounded && isWallJumping) return;
        
        //Horizontal movement
        rb.velocity += new Vector2(moveInput * (moveSpeed), 0);

        if (Mathf.Abs(moveInput) > 0) isWallJumping = false;
        
        if (moveInput == 0 && !isWallJumping)
        {
            //Apply decelerationRate
            float deceleration = decelerationRate;
            if (!isGrounded) deceleration = decelerationRate * 0.1f;
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, deceleration), rb.velocity.y);
        }

        //Clamp horizontal movement
        float x = rb.velocity.x;
        rb.velocity = new Vector2(Mathf.Clamp(x, -maxSpeed, maxSpeed), rb.velocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
    }

    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(-7f, 1f, 0f);
    }
}
