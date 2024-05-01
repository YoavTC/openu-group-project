using System;
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

    #region Value Testing
    public void SetDecelRate(float newValue) => decelerationRate = newValue;
    public void SetAirDecelRate(float newValue) => airDecelerationRateMultiplier = newValue;
    public void SetMaxSpeed(float newValue) => maxSpeed = newValue;
    public void SetJumpForce(float newValue) => jumpForce = newValue;
    public void SetWallJumpForce(float newValue) => wallJumpForce = newValue;

    [SerializeField] private TMP_Text statsDisplay;
    
    private void UpdateStatsDisplay()
    {
        statsDisplay.text = "decel rate: " + decelerationRate
                                           + "\nair decel rate: " + airDecelerationRateMultiplier
                                           + "\nmax speed: " + maxSpeed
                                           + "\njump force: " + jumpForce
                                           + "\nwall jump force: " + wallJumpForce;
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GUIUtility.systemCopyBuffer = statsDisplay.text;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.position = new Vector3(50, 6, 0);
        }
    }
    #endregion
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateStatsDisplay();
        
        //Horizontal movement
        if (transform.position.x < 56.8f)
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
        if (isGrounded && Input.GetButtonDown("Slide"))
        {
            //Implement slide state
        }

        //Testing death event
        if (transform.position.y < -7) Respawn();
    }

    private void FixedUpdate()
    {
        //Checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, wallGroundLayer);
        
        Vector3 transformPosition = transform.position;
        mountedRightWall =
            Physics2D.OverlapPoint(transformPosition + new Vector3(0.25f, 0f, 0f), wallGroundLayer);
        
        mountedLeftWall =
            Physics2D.OverlapPoint(transformPosition + new Vector3(-0.25f, 0f, 0f), wallGroundLayer);

        if (isGrounded) isWallJumping = false;
        if (Mathf.Abs(moveInput) > 0) isWallJumping = false;
        
        //Apply horizontal movement
        rb.velocity += new Vector2(moveInput * (moveSpeed), 0);
        
        //Apply deceleration
        if (moveInput == 0 && !isWallJumping)
        {
            float deceleration = decelerationRate;
            if (!isGrounded) deceleration = decelerationRate * airDecelerationRateMultiplier;
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