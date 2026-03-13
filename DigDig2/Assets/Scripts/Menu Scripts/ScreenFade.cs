using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] SpriteRenderer mySprite;

    void Awake()
    {
        mySprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1);
        Color targetColor = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0);

        yield return FadeCoroutine(startColor, targetColor, duration);

        gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0);
        Color targetColor = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1);

        yield return FadeCoroutine(startColor, targetColor, duration);

        gameObject.SetActive(false);

    }

    public IEnumerator FadeCoroutine(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            mySprite.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}
