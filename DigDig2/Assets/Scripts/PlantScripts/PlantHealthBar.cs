using UnityEngine;

public class PlantHealthBar : MonoBehaviour
{
    [SerializeField] PlantDeath HealthScript;
    float hpPercent;
    Vector3 scale;

    void Start()
    {
        scale = transform.localScale;
    }

    void Update()
    {
        hpPercent = HealthScript.hp;
        scale.x = hpPercent / HealthScript.hpMax;
        transform.localScale = scale;
    }
}
