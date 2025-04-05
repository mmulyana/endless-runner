using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    private SpriteRenderer sr;


    public bool playerUnlocked;

    [Header("Speed Info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float milestoneIncreaser;
    private float speedMilestone;
    private float defaultSpeed;
    private float defaultMilestoneIncreaser;


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

    [Header("Knockback Info")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnockback;
    private bool canBeKnocked = true;

    // Dead
    private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        CheckAnimation();

        slideTimerCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if(isKnockback || isDead)
        {
            return;
        }

        if (isGrounded)
        {
            doubleJump = true;
        }

        if (runBegin)
        {
            PlayerMove();
        }

        CheckForSlide();
        CheckInput();
        CheckForLedge();
        SpeedController();
    }

    public void Damage()
    {
        if(moveSpeed >= maxSpeed)
        {
            KnockBack();
        } else
        {
            StartCoroutine(Die());
        }
    }

    #region Die
    private IEnumerator Die()
    {
        isDead = true;
        canBeKnocked = false;
        rb.linearVelocity = knockbackDir;
        anim.SetBool("isDead", true);

        yield return new WaitForSeconds(.5f);
        rb.linearVelocity = new Vector2(0, 0);
    }

    private IEnumerator Invicibility()
    {
        Color originalColor = sr.color;
        Color darkenColor = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);

        canBeKnocked = false;

        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);

        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);
        canBeKnocked = true;
    }
    #endregion Die

    #region Knockback
    private void KnockBack()
    {
        if(!canBeKnocked)
        {
            return;
        }

        StartCoroutine(Invicibility());
        isKnockback = true;
        rb.linearVelocity = knockbackDir;
    }

    private void CancelKnockBack() => isKnockback = false;
    #endregion Knockback

    #region Speed
    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncreaser;
    }

    private void SpeedController()
    {
        if(moveSpeed == maxSpeed)
        {
            return;
        }

        if(transform.position.x > speedMilestone)
        {
            speedMilestone += milestoneIncreaser;

            moveSpeed *= speedMultiplier;
            milestoneIncreaser *= speedMultiplier;

            if(moveSpeed > maxSpeed)
            {
                moveSpeed = maxSpeed;
            }
        }
    }
    #endregion Speed

    #region Ledge
    private void CheckForLedge()
    {
        if(ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            rb.gravityScale = 0;

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

    private void LedgeClimbOver()
    {
        canClimb = false;
        rb.gravityScale = 5;

        transform.position = climbOverPosition;
        Invoke("AllowedLedgeGrab", .1f);
    }

    private void AllowedLedgeGrab() => canGrabLedge = true;
    #endregion Ledge

    #region Slide
    private void CheckForSlide()
    {
        if(slideTimerCounter < 0 && !ceillingDetected )
        {
            isSliding = false;
        }
    }

    private void SlideMove()
    {
        if(rb.linearVelocityX != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimerCounter = slideTimer;
            slideCooldownCounter = slideCooldown;
        }
    }
    #endregion Slide

    #region Animation
    private void CheckAnimation()
    {
        anim.SetBool("canDoubleJump", doubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);
        anim.SetBool("isKnocked", isKnockback);

        if(rb.linearVelocityY < -20)
        {
            anim.SetBool("canRoll", true);
        }
    }

    private void RollAnimFinished() => anim.SetBool("canRoll", false);
    #endregion Animation

    private void PlayerMove()
    {
        if(wallDetected)
        {
            SpeedReset();
            return;
        }
        if(isSliding)
        {
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocityY);
            return;
        }
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
    }

    private void playerJump()
    {
        if (isSliding)
        {
            return;
        }

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
        else if (doubleJump)
        {
            doubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJoumForce);
        }
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    runBegin = true;
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlideMove();
        }

        if (Input.GetKeyDown(KeyCode.D) && !isDead)
        {
            StartCoroutine(Die());
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            KnockBack();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y - groundCheckDistance));

        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
    }
}
