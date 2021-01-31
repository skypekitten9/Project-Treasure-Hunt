using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Checks for obstacles or end of platform")]
    public Transform obstacleDetection;
    public float rayDistance;

    [Header("Movement values")]
    public float speed;

    [Header("Attack values")]
    public float knockbackForce;
    public float attackDamage;

    private float knockupTimer;
    private float knockupTimerReset;

    Rigidbody2D rb;

    public LayerMask whatIsTerrain;

    private bool goingRight;

    enum BehaviourState
    {
        inactive,
        patrolling
    }

    BehaviourState behaviourState;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        behaviourState = BehaviourState.patrolling;
        goingRight = true;

        knockupTimer = 1.5f;
        knockupTimerReset = knockupTimer;
    }

    void Update()
    {
        ObstacleCheck();

        switch (behaviourState)
        {
            case BehaviourState.patrolling:

                Patrol();

                break;

            case BehaviourState.inactive:

                knockupTimer -= Time.deltaTime;

                if (knockupTimer <= 0)
                {
                    knockupTimer = knockupTimerReset;

                    behaviourState = BehaviourState.patrolling;
                }

                break;
        }
    }

    void Patrol()
    {
        if (goingRight)
        {
            rb.velocity = new Vector2(speed * 1, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(speed * -1, rb.velocity.y);
        }
    }

    void ObstacleCheck()
    {
        RaycastHit2D obstacleData = Physics2D.Raycast(obstacleDetection.position, Vector2.down, rayDistance);
        RaycastHit2D forwardData;

        if (goingRight)
        {
            forwardData = Physics2D.Raycast(obstacleDetection.position, Vector2.right, rayDistance, whatIsTerrain);
        }
        else
        {
            forwardData = Physics2D.Raycast(obstacleDetection.position, Vector2.left, rayDistance, whatIsTerrain);
        }

        if (obstacleData.collider == false || forwardData.collider == true && obstacleData.collider == true)
        {
            goingRight = !goingRight;

            if (goingRight)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    void Attack(GameObject target)
    {
        target.GetComponent<Movement>().ResetDamageTimer();
        target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (target.transform.position.x < transform.position.x)
        {
            if (Binoculars.Instance.State == BinocularsState.equiped)
            {
                target.GetComponent<Movement>().rb.AddForce(Vector2.left * knockbackForce * 2.5f);
                GameMaster.Instance.DecreaseLife();
            }
            else if (Binoculars.Instance.State == BinocularsState.reversed)
            {
                target.GetComponent<Movement>().rb.AddForce(Vector2.left * knockbackForce * 0.5f);
                GameMaster.Instance.DecreaseLife();
            }
            else
            {
                target.GetComponent<Movement>().rb.AddForce(Vector2.left * knockbackForce);
                GameMaster.Instance.DecreaseLife();
            }
        }
        else
        {
            if (Binoculars.Instance.State == BinocularsState.equiped)
            {
                target.GetComponent<Movement>().rb.AddForce(Vector2.right * knockbackForce * 5f);
                GameMaster.Instance.DecreaseLife();
            }
            else if (Binoculars.Instance.State == BinocularsState.reversed)
            {
                target.GetComponent<Movement>().rb.AddForce(Vector2.right * knockbackForce * 0.35f);
                GameMaster.Instance.DecreaseLife();
            }
            else
            {
                target.GetComponent<Movement>().rb.AddForce(Vector2.right * knockbackForce);
                GameMaster.Instance.DecreaseLife();
            }
        }

        if (Binoculars.Instance.State == BinocularsState.equiped)
        {
            target.GetComponent<Movement>().rb.AddForce(Vector2.up * knockbackForce  * 5);
        }
        else if (Binoculars.Instance.State == BinocularsState.reversed)
        {
            target.GetComponent<Movement>().rb.AddForce(Vector2.up * (knockbackForce / 8));
        }
        else
        {
            target.GetComponent<Movement>().rb.AddForce(Vector2.up * (knockbackForce / 2));
        }
    }

    public void Knockup()
    {
        if (behaviourState == BehaviourState.patrolling)
        {
            rb.velocity = Vector2.zero;
            behaviourState = BehaviourState.inactive;
            rb.AddForce(Vector2.up * 250);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<Movement>().isDamaged)
        {
            Attack(collision.gameObject);
        }
    }
}
