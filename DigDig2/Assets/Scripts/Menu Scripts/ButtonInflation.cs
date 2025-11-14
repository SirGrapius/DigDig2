using System.Collections;
using UnityEngine;

public class ButtonInflation : MonoBehaviour
{
    [SerializeField] bool inflate;
    [SerializeField] float lerpAmount;
    [SerializeField] AudioClip myClip;

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

    public void MouseEnter()
    {
        inflate = true;
        source.Play();
    }
    public void MouseExit()
    {
        inflate = false;
    }
    public void MouseClicked()
    {
        StartCoroutine(ButtonClicked());
    }

    IEnumerator ButtonClicked()
    {
        myTransform.localScale = Vector2.Lerp(enterSize, myTransform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        yield return new WaitForSeconds(0.1f);
        myTransform.localScale = Vector2.Lerp(originalSize, myTransform.localScale, Mathf.Pow(0.5f, Time.deltaTime * lerpAmount));
        yield return null;
    }
}
