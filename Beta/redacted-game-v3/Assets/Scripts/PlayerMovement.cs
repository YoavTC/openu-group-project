using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
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
    [SerializeField] private float moveSpeed;
    [SerializeField] private float decelerationRate, airDecelerationRateMultiplier, maxSpeed;
    private float moveInput;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [ReadOnly] [SerializeField] private bool canJump;
    [ReadOnly] [SerializeField] private bool isGrounded;
    private LayerMask groundLayers;
    private bool isJumpingThisFrame;

    [Header("Wall Jumps")] 
    [SerializeField] private float wallJumpForce;
    [SerializeField] private Transform rightWallPoint, leftWallPoint;
    [ReadOnly] [SerializeField] private bool mountedRightWall, mountedLeftWall, isWallJumping;

    [Header("Sliding")]
    [SerializeField] private Transform slideBlockPoint;
    [SerializeField] private Vector2 slideBlockRadius;
    [SerializeField] private float slideDurationMax, slideDurationMin;
    [ReadOnly] [SerializeField] private bool isSliding;
    [ReadOnly] [SerializeField] private bool canGetUp;
    private Coroutine slideCoroutine;

    [Header("Flip")]
    private int isFacingRight = 1;

    [Header("Events")]
    public UnityEvent playerDied;

    [Header("Animator Variables")]
    private bool is_idle, is_mounted, is_airborne;
    #endregion

    private void Start()
    {
        canJump = true;
        groundLayers = LayerMask.GetMask("NonJumpable", "WallGround");
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        isJumpingThisFrame = Input.GetButtonDown("Jump");
        HandleMovement();
        HandleFlipping();
        UpdateAnimatorStates();
        
        if (doVoidDeath && transform.position.y < voidDeathLevel) playerDied?.Invoke();
    }

    private void FixedUpdate()
    {
        PerformChecks();
        ApplyHorizontalMovement();
        ApplyDeceleration();
        ClampHorizontalMovement();
    }

    private void HandleMovement()
    {
        HandleHorizontalMovement();
        HandleJumping();
        HandleWallJumping();
        HandleSliding();
    }

    #region FixedUpdate Methods

    private void PerformChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        // mountedRightWall = facingDirection == 1 && (bool) Physics2D.OverlapPoint(rightWallPoint.position, wallGroundLayer);
        // mountedLeftWall = facingDirection == -1 && (bool) Physics2D.OverlapPoint(leftWallPoint.position, wallGroundLayer);
        
        mountedRightWall = Physics2D.OverlapPoint(rightWallPoint.position, wallGroundLayer);
        mountedLeftWall = Physics2D.OverlapPoint(leftWallPoint.position, wallGroundLayer);

        if (isGrounded) isWallJumping = false;
        //if (Mathf.Abs(moveInput) > 0) isWallJumping = false;
    }
    
    private void ApplyHorizontalMovement()
    {
        rb.velocity += new Vector2(moveInput * moveSpeed, 0);
    }
    
    private void ApplyDeceleration()
    {
        if (moveInput == 0 && !isWallJumping)
        {
            if (snappyMovement)
            {
                // Snappy movement DO NOT DELETE!
                if (isSliding) rb.velocity = new Vector2(isFacingRight * 10, rb.velocity.y);
                else
                {
                    rb.velocity = new Vector2(moveInput, rb.velocity.y);
                }
            }
            else
            {
                // Floaty movement DO NOT DELETE!
                float deceleration = decelerationRate;

                if (!isGrounded) deceleration = decelerationRate * airDecelerationRateMultiplier;
                if (isSliding) rb.velocity = new Vector2(isFacingRight * 10, rb.velocity.y);
                else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, deceleration), rb.velocity.y);
            }
        }
    }
    
    private void ClampHorizontalMovement()
    {
        float x = rb.velocity.x;
        rb.velocity = new Vector2(Mathf.Clamp(x, -maxSpeed, maxSpeed), rb.velocity.y);
    }

    #endregion

    #region Movement Methods

    private void HandleHorizontalMovement()
    {
        if (isWallJumping)
        {
            moveInput = 0;
            return;
        }
        if (!isSliding)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else moveInput = 0;
    }
    
    private void HandleJumping()
    {
        if (!isSliding)
        {
            if (isGrounded && isJumpingThisFrame && !isSliding && canJump)
            {
                InGameLogger.Log("jaumping!!!2!", Color.magenta);
                animator.SetTrigger("jump");
                StartCoroutine(JumpCooldown());
                Jump();
            } 
        }
    }
    
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
    }
    
    //Jump cooldown
    private IEnumerator JumpCooldown()
    {
        canJump = false;
        animator.SetBool("can_transition_from_jump", false);
        yield return HelperFunctions.GetWait(jumpCooldown);
        animator.SetBool("can_transition_from_jump", true);
        canJump = true;
    }
    
    private void HandleWallJumping()
    {
        if (CanJump())
        {
            animator.SetBool("is_mounted", true);
            Jump();
            rb.velocity = Vector2.zero;
            isWallJumping = true;

            if (mountedRightWall)
            {
                rb.velocity = new Vector2(-1 * wallJumpForce, 0);
                if (isFacingRight == 1) Flip();
            }
            if (mountedLeftWall)
            {
                rb.velocity = new Vector2(wallJumpForce, 0);
                if (isFacingRight == -1) Flip();
            }
        }
    }
    
    private void HandleSliding()
    {
        if (isJumpingThisFrame) return;
        if (isGrounded && Input.GetButtonDown("Slide") && !isSliding && Mathf.Abs(moveInput) > 0)
        {
            slideCoroutine = StartCoroutine(Slide());
        }

        if (!Input.GetButton("Slide") && isSliding && canGetUp)
        {
            StartCoroutine(GetUpFromSlide());
        }
    }

    private IEnumerator Slide()
    {
        InGameLogger.Log("sliding, wow!!1!", Color.green);
        // Animation
        animator.SetBool("is_sliding", true);

        canGetUp = false;
        isSliding = true;
        float timeDiff = slideDurationMax - slideDurationMin;

        yield return new WaitForSeconds(slideDurationMin);
        canGetUp = true;

        yield return new WaitForSeconds(timeDiff);
        //if (isSliding) GetUpFromSlide();
        if (isSliding) StartCoroutine(GetUpFromSlide());
    }
    
    private IEnumerator GetUpFromSlide()
    {
        // If under object and cannot exit slide
        bool cant = Physics2D.OverlapBox(slideBlockPoint.position, slideBlockRadius, 0f, wallGroundLayer);
        
        while (cant)
        {
            cant = Physics2D.OverlapBox(slideBlockPoint.position, slideBlockRadius, 0f, wallGroundLayer);
            //Debug.Log("Can't get up, retrying.. ");
            yield return HelperFunctions.GetWait(0.05f);
        }

        // Animation
        animator.SetBool("is_sliding", false);

        StopCoroutine(slideCoroutine);
        canGetUp = false;
        isSliding = false;
    }

    #endregion

    #region Helper Methods

    // Handles flipping the player's sprite based on movement direction
    private void HandleFlipping()
    {
        if (isWallJumping) return;
        if (moveInput > 0 && isFacingRight == -1)
        {
            Flip();
        }
        if (moveInput < 0 && isFacingRight == 1)
        {
            Flip();
        }
    }

    private bool CanJump() => !isGrounded && isJumpingThisFrame && (mountedRightWall || mountedLeftWall);
    
    

    // Updates animator states based on player's status
    private void UpdateAnimatorStates()
    {
        bool isIdleNow = moveInput == 0;
        if (is_idle != isIdleNow)
        {
            is_idle = isIdleNow;
            animator.SetBool("is_idle", is_idle);
        }
        
        bool isMountedNow = mountedLeftWall || mountedRightWall;
        if (is_mounted != isMountedNow)
        {
            is_mounted = isMountedNow;
            animator.SetBool("is_mounted", is_mounted);
        }
        
        bool isAirborneNow = !isGrounded;
        if (is_airborne != isAirborneNow)
        {
            is_airborne = isAirborneNow;
            animator.SetBool("is_airborne", is_airborne);
        }
    }

    // Flips the player's sprite direction
    private void Flip()
    {
        isFacingRight *= -1;
        Vector3 scale = animator.transform.localScale;
        scale.x *= -1;
        animator.transform.localScale = scale;
    }

    public void Respawn(Vector2 newSpawnPoint)
    {
        rb.velocity = Vector2.zero;
        transform.position = newSpawnPoint;
    }

    #endregion
    
    //Debug Panel
    public DebugInformation[] GetDebugInformation()
    {
        List<DebugInformation> debugInformations = new List<DebugInformation>();
        
        //Movement variables
        debugInformations.Add(new DebugInformation(nameof(moveInput), moveInput));
        debugInformations.Add(new DebugInformation(nameof(isGrounded), isGrounded));
        debugInformations.Add(new DebugInformation(nameof(canJump), canJump));
        debugInformations.Add(new DebugInformation(nameof(isJumpingThisFrame), isJumpingThisFrame));
        debugInformations.Add(new DebugInformation(nameof(mountedLeftWall), mountedLeftWall));
        debugInformations.Add(new DebugInformation(nameof(mountedRightWall), mountedRightWall));
        debugInformations.Add(new DebugInformation(nameof(isSliding), isSliding));
        debugInformations.Add(new DebugInformation(nameof(canGetUp), canGetUp));
        debugInformations.Add(new DebugInformation(nameof(isFacingRight), isFacingRight));

        //Sort & return
        return debugInformations
            .OrderBy(info => info.value is float || info.value is int ? 0 : 1)
            .ThenBy(info => info.value is int ? 0 : 1)
            .ToArray();
    }
}
