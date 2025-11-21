using System.Collections;
using UnityEngine;

public class ButtonInflation : MonoBehaviour
{
    [SerializeField] bool inflate;
    [SerializeField] float lerpAmount;
    [SerializeField] AudioClip myClip;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] string whatButton;

    [SerializeField] AudioSource source;
    [SerializeField] Vector2 enterSize;
    [SerializeField] Vector2 originalSize;
    RectTransform myTransform;

    void Start()
    {
        myTransform = GetComponent<RectTransform>();
        enterSize.x = myTransform.localScale.x * 1.1f;
        enterSize.y = myTransform.localScale.y * 1.1f;
        originalSize = myTransform.localScale;
        source = GetComponentInParent<AudioSource>();
        source.clip = myClip;
        sceneLoader = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoader>();
    }


    void Update()
    {
        if (inflate)
        {
            myTransform.localScale = Vector2.Lerp(enterSize, myTransform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        }
        else
        {
            myTransform.localScale = Vector2.Lerp(originalSize, myTransform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("mouseover");
        inflate = true;
        source.Play();
    }
    private void OnMouseExit()
    {
        Debug.Log("mouseexit");
        inflate = false;
    }
    private void OnMouseDown()
    {
        Debug.Log("mouseclick");
        StartCoroutine(ButtonClicked());
    }

    IEnumerator ButtonClicked()
    {
        myTransform.localScale = Vector2.Lerp(enterSize, myTransform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        yield return new WaitForSeconds(0.2f);
        myTransform.localScale = Vector2.Lerp(originalSize, myTransform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        yield return new WaitForSeconds(0.1f);
        sceneLoader.WhatButton(whatButton);
        yield return null;
    }
}
