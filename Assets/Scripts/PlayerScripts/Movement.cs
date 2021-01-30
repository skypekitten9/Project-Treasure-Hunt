using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Movement Values")]
    public float speed;
    public float jumpForce;
    public int extraJumps;
    public float jumpTimer;
    private int jumpReset;

    private float moveInput;
    private float checkRadius = 0.25f;
    private float jumpTimerReset;
    private float damageTimer;
    private float damageTimerReset;

    private bool isGrounded;
    private bool isJumping;
    public bool isDamaged;


    Vector2 velocity;

    Animator animator;

    public Rigidbody2D rb;

    public Transform groundCheck;

    public LayerMask whatIsGround;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        jumpReset = extraJumps;
        jumpTimerReset = jumpTimer;
        damageTimer = 0.35f;
        damageTimerReset = damageTimer;
        isDamaged = false;
    }

    void Update()
    {
        if (damageTimer >= 0)
        {
            isDamaged = true;
        }
        else
        {
            isDamaged = false;
        }

        if (!isDamaged)
        {
            Move();

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                jumpTimer = jumpTimerReset;
            }

            if (Input.GetKey(KeyCode.Space) && isJumping)
            {
                if (jumpTimer > 0)
                {
                    Jump();
                    jumpTimer -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }


            if (isGrounded)
            {
                extraJumps = jumpReset;
            }
        }

        damageTimer -= Time.deltaTime;
    }

    private void Move()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        moveInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            animator.SetBool("isRunning", true);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            animator.SetBool("isRunning", true);
        }
        else if (moveInput == 0)
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 1 * jumpForce);
        isJumping = true;
    }

    public void ResetDamageTimer()
    {
        damageTimer = damageTimerReset;
    }
}
