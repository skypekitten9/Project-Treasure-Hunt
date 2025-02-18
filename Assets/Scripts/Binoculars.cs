﻿using System.Collections;
using UnityEngine;

public enum BinocularsState { unequiped, equiped, reversed };
public class Binoculars : MonoBehaviour
{
    private static Binoculars instance = null;
    public static Binoculars Instance { get { return instance; } }
    public BinocularsState State { get { return state; } }
    public GameObject equipedModel, unequipedModel, reversedModel, sound;



    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }


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
        //equip or turn if possible
        if(Input.GetMouseButtonDown(0) && !busy)
        {
            AkSoundEngine.SetSwitch("PlayerSize", "Big", sound);
            equipedModel.SetActive(true);
            reversedModel.SetActive(false);
            unequipedModel.SetActive(false);

            if (state == BinocularsState.reversed)
            {
                TurnBinoculars();
            }
            else if (state == BinocularsState.unequiped)
            {
                reversed = false;
                ToggleBinoculars();
            }
            
            //if (state == BinocularsState.unequiped)
            //{
            //    reversed = false;
            //    ToggleBinoculars();
            //}
            //else
            //{
               
            //    TurnBinoculars();
            //}

        }

        //unequip if possible
        else if(Input.GetMouseButtonDown(1) && !busy)
        {
            AkSoundEngine.SetSwitch("PlayerSize", "Small", sound);
            equipedModel.SetActive(false);
            reversedModel.SetActive(true);
            unequipedModel.SetActive(false);
            if (state == BinocularsState.equiped)
            {
                TurnBinoculars();
            }
            else if(state == BinocularsState.unequiped)
            {
                reversed = true;
                ToggleBinoculars();
            }
            //if (state == BinocularsState.equiped || state == BinocularsState.reversed)
            //{
            //    ToggleBinoculars();
            //}
        }

        else if (Input.GetMouseButtonDown(2) && !busy)
        {
            AkSoundEngine.SetSwitch("PlayerSize", "Normal", sound);
            equipedModel.SetActive(false);
            reversedModel.SetActive(false);
            unequipedModel.SetActive(true);
            if (state != BinocularsState.unequiped)
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
                StartCoroutine(Scale(scaleDown, state));
                state = BinocularsState.reversed; //equiped
                AkSoundEngine.PostEvent("BigUnequip", this.gameObject);
            }
            else
            {
                StartCoroutine(Scale(scaleUp, state));
                state = BinocularsState.equiped; //reversed
                AkSoundEngine.PostEvent("SmallUnequip", this.gameObject);
            }
           
        }
        else
        {
            if(state == BinocularsState.equiped) AkSoundEngine.PostEvent("BigUnequip", this.gameObject);
            else AkSoundEngine.PostEvent("SmallUnequip", this.gameObject);
            StartCoroutine(Scale(originalScale, state)); //unqeuip 
            state = BinocularsState.unequiped;
        }
    }

    void TurnBinoculars()
    {
        busy = true;
        if (reversed)
        {
            StartCoroutine(Scale(scaleUp, state));
            state = BinocularsState.equiped; //equiped
            reversed = false;
            AkSoundEngine.PostEvent("SmallUnequip", this.gameObject);
        }
        else
        {
            StartCoroutine(Scale(scaleDown, state));
            state = BinocularsState.reversed; //reversed
            reversed = true;
            AkSoundEngine.PostEvent("BigUnequip", this.gameObject);
        }
    }

    private IEnumerator Scale(Vector2 targetScale, BinocularsState currentState)
    {
        bool revert = false;
        Vector2 initialScale = gameObject.transform.localScale;
        float progress = 0;
        while(progress <= 1)
        {
            if(cancelScale && !revert && targetScale.x > initialScale.x)
            {
                targetScale = initialScale;
                initialScale = transform.localScale;
                progress = 0;
                revert = true;
            }
            transform.localScale = Vector2.Lerp(initialScale, targetScale, progress);
            progress += Time.deltaTime * timeScale;
            yield return null;
        }
        transform.localScale = targetScale;
        busy = false;

        if (revert)
        {
            state = currentState;
            if (state != BinocularsState.unequiped) reversed = !reversed;
        }
    }

    public void CancelScale(bool cancel)
    {
        cancelScale = cancel;
    }
}
