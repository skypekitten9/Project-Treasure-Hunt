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

    Rigidbody2D rb;

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

        if (obstacleData.collider == false)
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
        //GameMaster.Instance.DecreaseLife();

        if (target.transform.position.x < transform.position.x)
        {
            target.GetComponent<Movement>().rb.AddForce(Vector2.left * knockbackForce);
        }
        else
        {
            target.GetComponent<Movement>().rb.AddForce(Vector2.right * knockbackForce);
        }

        target.GetComponent<Movement>().rb.AddForce(Vector2.up * (knockbackForce / 2));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Attack(collision.gameObject);
        }
    }
}
