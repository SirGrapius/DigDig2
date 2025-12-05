using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class AidsSlider : MonoBehaviour
{

    //Use the same method as enemy spawning to determain the min and max values through the a new object that's the same size as the fillobject
    //then use localposition to change the handleobject's x position to the cursor's x position
    //this script goes on the handle
    [SerializeField] RectTransform fillTransform;
    [SerializeField] GameObject backgroundObject;
    [SerializeField] GameObject handleObject;
    [SerializeField] CanvasScaler scaler;

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
        scaler = GetComponentInParent<CanvasScaler>();
    }

    
    void Update()
    {
        if (inflate)
        {
            handleObject.transform.localScale = Vector2.Lerp(enterSize, handleObject.transform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        }
        else
        {
            handleObject.transform.localScale = Vector2.Lerp(originalSize, handleObject.transform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        }

        if (beingDraged && handleObject.transform.position.x !> handleMax + 0.5f && handleObject.transform.position.x !< handleMin - 0.5f)
        {
            handleObject.transform.localPosition = new Vector3(Input.mousePosition.x/scaler.scaleFactor, handleObject.transform.position.y, handleObject.transform.position.z);
            fillTransform.localScale = new Vector3(fillTransform.localScale.x * (handleObject.transform.position.x / fillTransform.localScale.x), fillTransform.localScale.y, fillTransform.localScale.z);
        }

        if (handleObject.transform.position.x > handleMax)
        {
            transform.position = new Vector3(handleMax, transform.position.y, transform.position.z);
        }
        if (handleObject.transform.position.x < handleMin)
        {
            transform.position = new Vector3(handleMin, transform.position.y, transform.position.z);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("mouseover");
        inflate = true;
    }
    private void OnMouseExit()
    {
        Debug.Log("mouseexit");
        inflate = false;
    }
    private void OnMouseDown()
    {
        Debug.Log(Input.mousePosition.x / scaler.scaleFactor);
        beingDraged = true;
    }
    private void OnMouseUp()
    {
        Debug.Log(Input.mousePosition.x / scaler.scaleFactor);
        beingDraged = false;
    }
}
