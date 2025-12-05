using System.Collections;
using UnityEngine;

public class AidsSlider : MonoBehaviour
{
    //this script goes on the handle
    [SerializeField] RectTransform fillTransform;
    [SerializeField] GameObject backgroundObject;
    [SerializeField] GameObject handleObject;

    [SerializeField] float handleMax = 2;
    [SerializeField] float handleMin = -2;
    [SerializeField] bool inflate = false;
    [SerializeField] Vector2 enterSize;
    [SerializeField] Vector2 originalSize;
    [SerializeField] float lerpAmount;
    [SerializeField] bool beingDraged;
    
    void Start()
    {
        enterSize.x = transform.localScale.x * 1.1f;
        enterSize.y = transform.localScale.y * 1.1f;
        originalSize = transform.localScale;
    }

    
    void Update()
    {
        if (inflate)
        {
            transform.localScale = Vector2.Lerp(enterSize, transform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        }
        else
        {
            transform.localScale = Vector2.Lerp(originalSize, transform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        }

        if (beingDraged && transform.position.x < -2)
        {

        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("mouseover");
    }
    private void OnMouseExit()
    {
        Debug.Log("mouseexit");
    }
    private void OnMouseDown()
    {
        Debug.Log("mouseclick");
        beingDraged = true;
    }
    private void OnMouseUp()
    {
        Debug.Log("mouseletgo");
        beingDraged = false;
    }
}
