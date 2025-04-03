using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    public bool playerUnlocked;

    [Header("Move info")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float jumpForce = 15;
    [SerializeField] private float doubleJoumForce = 12;

    private bool doubleJump;
    public bool runBegin;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;
    [SerializeField] private float ceillingCheckDistance;
    private bool ceillingDetected;

    public bool ledgeDetected;


    private bool isGrounded;
    public bool wallDetected;

    [Header("Slide Info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCooldown;
    private float slideCooldownCounter;
    private float slideTimerCounter;
    private bool isSliding;

    [Header("Ledge Info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBeginPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAnimation();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if (isGrounded)
        {
            doubleJump = true;
        }

        if (runBegin && !wallDetected || isSliding)
        {
            PlayerMove();
        }

        CheckCollision();
        CheckForSlide();
        CheckInput();
        CheckForLedge();
    }

    private void CheckForLedge()
    {
        if(ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbBeginPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }

        if(canClimb)
        {
            transform.position = climbBeginPosition;
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            runBegin = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJump();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlideMove();
        }
    }

    private void CheckForSlide()
    {
        if(slideTimerCounter < 0 && !ceillingDetected )
        {
            isSliding = false;
        }
    }

    private void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = climbOverPosition;
        Invoke("AllowedLedgeGrab", .1f);
    }

    private void AllowedLedgeGrab() => canGrabLedge = true;

    private void SlideMove()
    {
        if(rb.linearVelocityX != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void CheckAnimation()
    {
        anim.SetBool("canDoubleJump", doubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);
    }

    private void PlayerMove()
    {
        if(isSliding)
        {
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocityY);
            return;
        }
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
    }

    private void playerJump()
    {
        if(isSliding)
        {
            return;
        }

        if(isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        } else if(doubleJump)
        {
            doubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJoumForce);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y - groundCheckDistance));

        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
    }
}
