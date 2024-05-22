using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float decelerationRate, airDecelerationRateMultiplier, maxSpeed;
    private float moveInput;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [ReadOnly] [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Wall Jumps")] 
    [SerializeField] private float wallJumpForce;
    [ReadOnly] [SerializeField] private bool mountedRightWall, mountedLeftWall, isWallJumping;

    [Header("Sliding")] 
    [ReadOnly] [SerializeField] private bool isSliding;
    [ReadOnly] [SerializeField] private bool canGetUp;
    [SerializeField] private float slidingDecelerationRateMultiplier;
    [SerializeField] private float slideDurationMax, slideDurationMin;
    private Coroutine slideCoroutine;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Horizontal movement
        if (transform.position.x < 56.8f && !isSliding)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else moveInput = 0;
       
        
        //Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        
        //Wall jumping
        if (!isGrounded && Input.GetButtonDown("Jump") && (mountedRightWall || mountedLeftWall))
        {
            Jump();
            rb.velocity = Vector2.zero;
            isWallJumping = true;
            
            if (mountedRightWall) rb.velocity = new Vector2(-1 * wallJumpForce, 0);
            if (mountedLeftWall) rb.velocity = new Vector2(wallJumpForce, 0);
        }
        
        //Sliding
        if (isGrounded && Input.GetButtonDown("Slide") && !isSliding)
        { ;
            slideCoroutine = StartCoroutine(Slide());
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.red;
            //transform.DORotate(new Vector3(0, 0, 70), 0.1f);
        }

        if (!Input.GetButton("Slide") && isSliding && canGetUp)
        {
            GetUpFromSlide();
        }

        //Testing death event
        if (transform.position.y < -7) Respawn();
    }

    private void FixedUpdate()
    {
        //Checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallGroundLayer);
        
        Vector3 transformPosition = transform.position;
        mountedRightWall = Physics2D.OverlapPoint(transformPosition + new Vector3(0.25f, 0f, 0f), wallGroundLayer);
        mountedLeftWall = Physics2D.OverlapPoint(transformPosition + new Vector3(-0.25f, 0f, 0f), wallGroundLayer);

        if (isGrounded) isWallJumping = false;
        if (Mathf.Abs(moveInput) > 0) isWallJumping = false;
        
        //Apply horizontal movement
        rb.velocity += new Vector2(moveInput * (moveSpeed), 0);
        
        //Apply deceleration
        if (moveInput == 0 && !isWallJumping)
        {
            float deceleration = decelerationRate;
            
            if (!isGrounded) deceleration = decelerationRate * airDecelerationRateMultiplier;
            if (isGrounded && isSliding) deceleration = decelerationRate * slidingDecelerationRateMultiplier;
            
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

    private IEnumerator Slide()
    {
        canGetUp = false;
        isSliding = true;
        float timeDiff = slideDurationMax - slideDurationMin;
        
        yield return new WaitForSeconds(slideDurationMin);
        canGetUp = true;
        
        yield return new WaitForSeconds(timeDiff);
        if (isSliding) GetUpFromSlide();
    }

    private void GetUpFromSlide()
    {
        StopCoroutine(slideCoroutine);
        canGetUp = false;
        isSliding = false;
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
        //transform.DORotate(Vector3.zero, 0.1f);
    }
}