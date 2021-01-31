using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBridgeScript : MonoBehaviour
{
    public Transform partOne, partTwo, partThree;
    public GameObject brokenOne, brokenTwo, brokenThree;

    private bool isBroken;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Movement>().isSlaming)
            {
                Instantiate(brokenOne, new Vector2(partOne.position.x, partOne.position.y), Quaternion.identity);
                Instantiate(brokenTwo, new Vector2(partTwo.position.x, partTwo.position.y), Quaternion.identity);
                Instantiate(brokenThree, new Vector2(partThree.position.x, partThree.position.y), Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}