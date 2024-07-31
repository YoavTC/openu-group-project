using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
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
    private PlayerSoundController playerSoundController;
    private Rigidbody2D rb;

    [HorizontalLine]
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float decelerationRate, airDecelerationRateMultiplier, maxSpeed;
    private float moveInput;

    [Header("Acceleration")] 
    [SerializeField] private Vector3 accelerationTimePoints;
    [SerializeField] private Vector3 accelerationBoostPoints;
    private float nonZeroVelocityTime;
    private float originalMaxSpeed;
    private int lastSpeedState;

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
    [SerializeField] private float wallJumpDuration;
    [SerializeField] private Transform rightWallPoint;
    [FormerlySerializedAs("mountedRightWall")] [ReadOnly] [SerializeField] private bool wallMounted;
    [ReadOnly] [SerializeField] private bool isWallJumping;
    private PlayerClimbingController playerClimbingController;

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
    public UnityEvent<int> speedLevelChange;

    [Header("Animator Variables")]
    private bool is_idle, is_mounted, is_airborne;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerClimbingController = GetComponent<PlayerClimbingController>();
        playerSoundController = GetComponent<PlayerSoundController>();

        lastSpeedState = 0;
        originalMaxSpeed = maxSpeed;
        canJump = true;
        groundLayers = LayerMask.GetMask("NonJumpable", "WallGround");
    }
    
    private void Update()
    {
        if (Time.deltaTime == 0) return;
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
        HandleAcceleration();
        HandleJumping();
        if (isJumpingThisFrame && CanWallJump()) HandleWallJumping();
        HandleSliding();
    }

    #region FixedUpdate Methods

    private void PerformChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
        wallMounted = Physics2D.OverlapPoint(rightWallPoint.position, wallGroundLayer);

        if ((wallMounted) && !isGrounded)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        else rb.gravityScale = 2f;

        if (isGrounded) isWallJumping = false;
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
                if (isSliding)
                {
                    rb.velocity = new Vector2((isFacingRight * maxSpeed) * 10, rb.velocity.y);
                } else
                {
                    rb.velocity = new Vector2(moveInput, rb.velocity.y);
                }
            }
            else
            {
                // Floaty movement DO NOT DELETE!
                float deceleration = decelerationRate;

                if (!isGrounded) deceleration = decelerationRate * airDecelerationRateMultiplier;
                if (isSliding) rb.velocity = new Vector2((isFacingRight * maxSpeed) * 10, rb.velocity.y);
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
    
    private void HandleAcceleration()
    {
        if (isSliding || isWallJumping || !isGrounded)
        {
            return;
        }

        if (Math.Abs(rb.velocity.x) > 0.5f)
        {
            nonZeroVelocityTime += Time.deltaTime;

            int newSpeedState = 0;
            float newMaxSpeed = originalMaxSpeed;

            if (nonZeroVelocityTime >= accelerationTimePoints.z)
            {
                newSpeedState = 3;
                newMaxSpeed += accelerationBoostPoints.z;
            }
            else if (nonZeroVelocityTime >= accelerationTimePoints.y)
            {
                newSpeedState = 2;
                newMaxSpeed += accelerationBoostPoints.y;
            }
            else if (nonZeroVelocityTime >= accelerationTimePoints.x)
            {
                newSpeedState = 1;
                newMaxSpeed += accelerationBoostPoints.x;
            }

            if (newSpeedState != lastSpeedState)
            {
                speedLevelChange?.Invoke(newSpeedState);
                lastSpeedState = newSpeedState;
            }
            maxSpeed = newMaxSpeed;
        }
        else
        {
            if (lastSpeedState != 0)
            {
                speedLevelChange?.Invoke(0);
                lastSpeedState = 0;
            }
            nonZeroVelocityTime = 0;
            maxSpeed = originalMaxSpeed;
        }
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
        playerSoundController.OnJump();
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
        animator.SetBool("is_mounted", true);
        
        rb.velocity = Vector2.zero;
        isWallJumping = true;
            
        Vector3 playerPos = transform.position;
        var (point, isLastPoint) = playerClimbingController.GetNextPoint(playerPos, groundLayers, isFacingRight);
        if (point == Vector3.zero || isLastPoint)
        {
            InGameLogger.Log("isFacingRight: " + isFacingRight, Color.red);
            rb.velocity = new Vector2((wallJumpForce + maxSpeed) * (isFacingRight * -1), 0);
            Jump();
        }
        else
        {
            transform.DOMove(point, wallJumpDuration).SetEase(Ease.OutSine);
        }
        Flip();
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
        playerSoundController.OnSlideActivate();
        // Animation
        animator.SetBool("is_sliding", true);

        canGetUp = false;
        isSliding = true;
        float timeDiff = slideDurationMax - slideDurationMin;

        yield return new WaitForSeconds(slideDurationMin);
        canGetUp = true;

        yield return new WaitForSeconds(timeDiff);
        if (isSliding) StartCoroutine(GetUpFromSlide());
    }
    
    private IEnumerator GetUpFromSlide()
    {
        // If under object and cannot exit slide
        bool cant = Physics2D.OverlapBox(slideBlockPoint.position, slideBlockRadius, 0f, groundLayers);
        
        while (cant)
        {
            cant = Physics2D.OverlapBox(slideBlockPoint.position, slideBlockRadius, 0f, groundLayers);
            //Debug.Log("Can't get up, retrying.. ");
            yield return HelperFunctions.GetWait(0.05f);
        }

        // Animation
        animator.SetBool("is_sliding", false);
        
        playerSoundController.OnSlideDeactivate();

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

    private bool CanWallJump() => !isGrounded && wallMounted;
    
    

    // Updates animator states based on player's status
    private void UpdateAnimatorStates()
    {
        bool isIdleNow = moveInput == 0;
        if (is_idle != isIdleNow)
        {
            is_idle = isIdleNow;
            animator.SetBool("is_idle", is_idle);
        }
        
        bool isMountedNow = wallMounted;
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
        //debugInformations.Add(new DebugInformation(nameof(isJumpingThisFrame), isJumpingThisFrame));
        debugInformations.Add(new DebugInformation(nameof(isWallJumping), isWallJumping));
        debugInformations.Add(new DebugInformation(nameof(wallMounted), wallMounted));
        // debugInformations.Add(new DebugInformation(nameof(isSliding), isSliding));
        // debugInformations.Add(new DebugInformation(nameof(canGetUp), canGetUp));
        debugInformations.Add(new DebugInformation(nameof(isFacingRight), isFacingRight));
        //debugInformations.Add(new DebugInformation(nameof(rb.velocity.x), rb.velocity.x));
        debugInformations.Add(new DebugInformation(nameof(nonZeroVelocityTime), nonZeroVelocityTime));
        debugInformations.Add(new DebugInformation(nameof(lastSpeedState), lastSpeedState));

        //Sort & return
        return debugInformations
            .OrderBy(info => info.value is float || info.value is int ? 0 : 1)
            .ThenBy(info => info.value is int ? 0 : 1)
            .ToArray();
    }
}
