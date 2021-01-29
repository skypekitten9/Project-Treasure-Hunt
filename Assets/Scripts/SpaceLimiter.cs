using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLimiter : MonoBehaviour
{
    public GameObject binocularObjectToLimit;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something hit player head!");
        binocularObjectToLimit.GetComponent<Binoculars>().CancelScale(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Head is clear!");
        binocularObjectToLimit.GetComponent<Binoculars>().CancelScale(false);
    }
}
