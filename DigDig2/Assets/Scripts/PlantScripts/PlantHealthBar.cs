using UnityEngine;

public class PlantHealthBar : MonoBehaviour
{
    [SerializeField] PlantDeath HealthScript;
    float hpPercent;
    Vector3 scale;
    SpriteRenderer mySpriteRenderer;
    Color negativeHp;
    Color positiveHp;

    void Start()
    {
        scale = transform.localScale;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        negativeHp = new Vector4(1, 0, 0.5f,1);
        negativeHp = new Vector4(0, 1, 0, 1);

    }

    void Update()
    {
        hpPercent = HealthScript.hp;
        scale.x = hpPercent / HealthScript.hpMax;
        transform.localScale = scale;
        if (scale.x < 0)
        {
            mySpriteRenderer.color = negativeHp;
        }
        else
        {
            mySpriteRenderer.color = positiveHp;
        }
    }
}
