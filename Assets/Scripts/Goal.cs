﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    bool transitioning = false;
    bool goalReached = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") goalReached = true;
    }

    private void Update()
    {
        if (goalReached && !transitioning)
        {
            transitioning = true;
            GameMaster.Instance.Win();
        }
    }
}
