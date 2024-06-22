using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Debug Options")] 
    [SerializeField] private bool doVoidDeath;
    [SerializeField] private bool snappyMovement;
    [EnableIf("doVoidDeath")]
    [SerializeField] private float voidDeathLevel;
    
    [HorizontalLine]
    [Header("Layers")]
    [SerializeField] private LayerMask wallGroundLayer;
    
    [Header("Components")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    
    [HorizontalLine]
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
    [SerializeField] private Transform rightWallPoint, leftWallPoint;
    [ReadOnly] [SerializeField] private bool mountedRightWall, mountedLeftWall, isWallJumping;

    [Header("Sliding")]
    [SerializeField] private Transform slideBlockPoint;
    [SerializeField] private float slideBlockRaycastDistance;
    [SerializeField] private Vector2 slideBlockRadius;
    [SerializeField] private float slideDurationMax, slideDurationMin;
    [ReadOnly] [SerializeField] private bool isSliding;
    [ReadOnly] [SerializeField] private bool canGetUp;
    private Coroutine slideCoroutine;

    [Header("Flip")]
    //private bool facingRight = true;
    private int facingDirection = 1;
    
    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Flipping
        if (moveInput > 0 && facingDirection == -1)
        {
            Flip();
        }
        if (moveInput < 0 && facingDirection == 1)
        {
            Flip();
        }
        
        //Horizontal movement
        if (!isSliding)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        } else moveInput = 0;
       
        
        //Jumping
        if (isGrounded && Input.GetButtonDown("Jump") && !isSliding)
        {
            animator.SetTrigger("jump");
            Jump();
        }
        
        //Wall jumping
        if (!isGrounded && Input.GetButtonDown("Jump") && (mountedRightWall || mountedLeftWall))
        {
            //Animator
            animator.SetBool("is_mounted", true);
            
            Jump();
            rb.velocity = Vector2.zero;
            isWallJumping = true;
            
            if (mountedRightWall) rb.velocity = new Vector2(-1 * facingDirection * wallJumpForce, 0);
            if (mountedLeftWall) rb.velocity = new Vector2(facingDirection * wallJumpForce, 0);
            Flip();
        }
        
        animator.SetBool("is_idle", !(Mathf.Abs(moveInput) > 0));
        
        //Wall Mounted
        animator.SetBool("is_mounted", mountedLeftWall || mountedRightWall);
        
        //Airborne
        animator.SetBool("is_airborne", !isGrounded);
        
        //Sliding
        if (isGrounded && Input.GetButtonDown("Slide") && !isSliding && Mathf.Abs(moveInput) > 0)
        { 
            slideCoroutine = StartCoroutine(Slide());
        }

        if (!Input.GetButton("Slide") && isSliding && canGetUp)
        {
            GetUpFromSlide();
        }

        //Testing death event
        if (doVoidDeath && transform.position.y < voidDeathLevel) Respawn();
    }

    private void FixedUpdate()
    {
        //Checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallGroundLayer);
        
        mountedRightWall = Physics2D.OverlapPoint(rightWallPoint.position, wallGroundLayer);
        mountedLeftWall = Physics2D.OverlapPoint(leftWallPoint.position, wallGroundLayer);

        if (isGrounded) isWallJumping = false;
        if (Mathf.Abs(moveInput) > 0) isWallJumping = false;
        
        //Apply horizontal movement
        rb.velocity += new Vector2(moveInput * (moveSpeed), 0);
        
        //Apply deceleration
        if (moveInput == 0 && !isWallJumping)
        {
            if (snappyMovement)
            {
                //Snappy movement DO NOT DELETE!
                if (isSliding) rb.velocity = new Vector2(facingDirection * 10, rb.velocity.y);
                else
                {
                    rb.velocity = new Vector2(moveInput, rb.velocity.y);
                }
            }
            else
            {
                //Floaty movement DO NOT DELETE!
                float deceleration = decelerationRate;
            
                if (!isGrounded) deceleration = decelerationRate * airDecelerationRateMultiplier;
                if (isSliding) rb.velocity = new Vector2(facingDirection * 10, rb.velocity.y);
                else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, deceleration), rb.velocity.y);
            }
        }

        //Clamp horizontal movement
        float x = rb.velocity.x;
        rb.velocity = new Vector2(Mathf.Clamp(x, -maxSpeed, maxSpeed), rb.velocity.y);
    }

    #region Movement

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
    }
    
    private IEnumerator Slide()
    {
        //Animation
        //animator.SetTrigger("slide");
        animator.SetBool("is_sliding", true);
        
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
        //If under object and cannot exit slide
        if (Physics2D.OverlapBox(slideBlockPoint.position, slideBlockRadius, 0f, wallGroundLayer)) { return; }
        
        //Animation
        //animator.SetTrigger("get_up");
        animator.SetBool("is_sliding", false);
        
        StopCoroutine(slideCoroutine);
        canGetUp = false;
        isSliding = false;
    }

    #endregion
    
    private Vector3 startPosition;

    private void Flip()
    {
        // Debug.Log("Flipped!");
        // float currentScale = transform.localScale.x;
        // transform.DOScaleX(currentScale * -1, 0f);
        // facingRight = !facingRight;
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Respawn()
    {
        playerDied?.Invoke();
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
    }
    
    //Events
    [Header("Events")]
    public UnityEvent playerDied;
}