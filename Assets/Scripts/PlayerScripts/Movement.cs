using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Movement Values")]
    public float speed;
    public float bigSpeed;
    public float smallSpeed;
    public float jumpForce;
    public int extraJumps;
    public float jumpTimer;
    private int jumpReset;

    private float originalSpeed;
    private float moveInput;
    private float checkRadius = 0.25f;
    private float jumpTimerReset;
    private float damageTimer;
    private float damageTimerReset;

    private bool isGrounded;
    private bool isJumping;
    public bool isDamaged;
    public bool isSlaming;
    public GameObject model;


    Vector2 velocity;

    Animator animator;

    public Rigidbody2D rb;

    public Transform groundCheck;

    public LayerMask whatIsGround;
    public LayerMask targetsToSlam;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = model.GetComponent<Animator>();

        jumpReset = extraJumps;
        jumpTimerReset = jumpTimer;
        damageTimer = 0.35f;
        damageTimerReset = damageTimer;
        isDamaged = false;
        originalSpeed = speed;
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

        AlterProperties();

        if (!isDamaged || !isSlaming)
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                Slam();
            }

            if (isGrounded)
            {
                extraJumps = jumpReset;
            }
        }

        if (isSlaming)
        {
            if (rb.velocity.y >= 0)
            {
                Physics2D.OverlapCircle(groundCheck.position, 5f, targetsToSlam).GetComponent<EnemyBehaviour>().Knockup();
                isSlaming = false;
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
        animator.Play("Player_Model_Jump");
        rb.velocity = new Vector2(rb.velocity.x, 1 * jumpForce);
        isJumping = true;
    }

    public void ResetDamageTimer()
    {
        damageTimer = damageTimerReset;
    }

    private void AlterProperties()
    {
        if (Binoculars.Instance.State == BinocularsState.equiped)
        {
            speed = bigSpeed;
            rb.mass = 5;
            rb.gravityScale = 2.5f;
            jumpForce = 7;
        }
        else if (Binoculars.Instance.State == BinocularsState.reversed)
        {
            speed = smallSpeed;
            rb.mass = 0.5f;
            rb.gravityScale = 0.5f;
            jumpForce = 2;
        }
        else if (Binoculars.Instance.State == BinocularsState.unequiped)
        {
            speed = originalSpeed;
            rb.mass = 1;
            rb.gravityScale = 1;
            jumpForce = 3;
        }
    }

    private void Slam()
    {
        if (!isGrounded && Binoculars.Instance.State == BinocularsState.equiped)
        {
            isSlaming = true;

            rb.velocity = Vector2.down * 5;
        }
    }
}
