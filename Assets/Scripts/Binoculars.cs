using System.Collections;
using UnityEngine;

public enum BinocularsState { unequiped, equiped, reversed };
public class Binoculars : MonoBehaviour
{
    public Vector2 scaleUp, scaleDown;
    public float timeScale;

    bool reversed, busy, cancelScale;
    BinocularsState state;
    Vector2 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBools();
        state = BinocularsState.unequiped;
        originalScale = gameObject.transform.localScale;
    }

    #region Initialization
    void InitializeBools()
    {
        reversed = false;
        busy = false;
        cancelScale = false;
    }
    #endregion

    void Update()
    {
        //test
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Scaling test!");
            StartCoroutine(Scale(scaleUp));
        }

        //equip or turn if possible
        if(Input.GetMouseButtonDown(0) && !busy)
        {
            if(state == BinocularsState.unequiped)
            {
                ToggleBinoculars();
            }
            else
            {
                TurnBinoculars();
            }
        }

        //unequip if possible
        else if(Input.GetMouseButtonDown(1) && !busy)
        {
            if (state == BinocularsState.equiped || state == BinocularsState.reversed)
            {
                ToggleBinoculars();
            }
        }
        
    }

    void ToggleBinoculars()
    {
        busy = true;
        if(state == BinocularsState.unequiped)
        {
            if (reversed)
            {
                StartCoroutine(Scale(scaleDown));
                state = BinocularsState.reversed; //equiped
            }
            else
            {
                StartCoroutine(Scale(scaleUp));
                state = BinocularsState.equiped; //reversed
            }
           
        }
        else
        {
            StartCoroutine(Scale(originalScale)); //unqeuip 
            state = BinocularsState.unequiped;
        }
    }

    void TurnBinoculars()
    {
        busy = true;
        if (reversed)
        {
            StartCoroutine(Scale(scaleUp));
            state = BinocularsState.equiped; //equiped
            reversed = false;
        }
        else
        {
            StartCoroutine(Scale(scaleDown));
            state = BinocularsState.reversed; //reversed
            reversed = true;
        }
    }

    private IEnumerator Scale(Vector2 targetScale)
    {
        Vector2 initialScale = gameObject.transform.localScale;
        float progress = 0;
        while(progress <= 1)
        {
            transform.localScale = Vector2.Lerp(initialScale, targetScale, progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }
        transform.localScale = targetScale;
        busy = false;
        cancelScale = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //cancelScale = true;
    }
}
