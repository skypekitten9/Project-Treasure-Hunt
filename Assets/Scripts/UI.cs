using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject[] hearts;
    private static UI instance = null;
    int heartsLeft;
    public static UI Instance { get { return instance; } }


    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        heartsLeft = hearts.Length;
    }

    public void DecreaseHeart()
    {
        if (heartsLeft > 0)
        {
            heartsLeft--;
            hearts[heartsLeft].SetActive(false);
        }
    }
}
