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

    private bool isGrounded;
    public bool wallDetected;

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

        if (isGrounded)
        {
            doubleJump = true;
        }

        if (runBegin && !wallDetected)
        {
            PlayerMove();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            runBegin = true;
        }

        CheckCollision();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJump();
        }
    }

    private void CheckAnimation()
    {
        anim.SetBool("canDoubleJump", doubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetFloat("xVelocity", rb.linearVelocityX);
    }

    private void PlayerMove()
    {
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, whatIsGround);
    }

    private void playerJump()
    {
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
    }
}
