using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    bool transitioning = false;
    bool deathZoneReached = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") deathZoneReached = true;
    }

    private void Update()
    {
        if (deathZoneReached && !transitioning)
        {
            Debug.Log("Hewo");
            transitioning = true;
            GameMaster.Instance.Loose();
        }
    }
}
