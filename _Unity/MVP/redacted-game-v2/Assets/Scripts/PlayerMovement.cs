using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Debug Options")] 
    [SerializeField] private bool doVoidDeath;
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
    [SerializeField] private float slideDurationMax, slideDurationMin;
    [ReadOnly] [SerializeField] private bool isSliding;
    [ReadOnly] [SerializeField] private bool canGetUp;
    private Coroutine slideCoroutine;
    
    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Horizontal movement
        if (!isSliding)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        } else moveInput = 0;
       
        
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
            float deceleration = decelerationRate;
            
            if (!isGrounded) deceleration = decelerationRate * airDecelerationRateMultiplier;
            if (isSliding) deceleration = 0;
            
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, deceleration), rb.velocity.y);
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
        animator.SetTrigger("slide");
        
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
        //bool forceSlide = Physics2D.OverlapPoint(upWallPoint.position, wallGroundLayer);
        bool forceSlide = Physics2D.Raycast(slideBlockPoint.position, Vector2.up, slideBlockRaycastDistance, wallGroundLayer);
        Debug.DrawRay(slideBlockPoint.position, Vector2.up * slideBlockRaycastDistance, Color.red ,0.5f);
        
        if (forceSlide)
        {
            return;
        }
        // while (forceSlide)
        // {
        //     forceSlide = Physics2D.OverlapPoint(upWallPoint.position, wallGroundLayer);
        //     yield return HelperFunctions.GetWait(0.1f);
        //     Debug.Log("stuck!");
        // }
        //Animation
        animator.SetTrigger("run");
        
        StopCoroutine(slideCoroutine);
        canGetUp = false;
        isSliding = false;
    }

    #endregion
    
    private Vector3 startPosition;

    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
    }

}