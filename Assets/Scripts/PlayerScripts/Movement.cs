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
    public GameObject model;


    Vector2 velocity;

    Animator animator;

    public Rigidbody2D rb;

    public Transform groundCheck;

    public LayerMask whatIsGround;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //animator = gameObject.GetComponent<Animator>();

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
           // animator.SetBool("isRunning", true);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            //animator.SetBool("isRunning", true);
        }
        else if (moveInput == 0)
        {
            //animator.SetBool("isRunning", false);
        }
    }

    private void Jump()
    {
        //AkSoundEngine.PostEvent("PlayerJump", model);
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
            jumpForce = 3;
        }
        else if (Binoculars.Instance.State == BinocularsState.reversed)
        {
            speed = smallSpeed;
            rb.mass = 0.25f;
            rb.gravityScale = 0.5f;
            jumpForce = 5;
        }
        else if (Binoculars.Instance.State == BinocularsState.unequiped)
        {
            speed = originalSpeed;
            rb.mass = 1;
            rb.gravityScale = 1;
            jumpForce = 3;
        }
    }
}
