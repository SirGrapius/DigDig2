using UnityEngine;

public class PotatoScript : MonoBehaviour
{
    PlantDeath Life;
    [SerializeField] int damageValue = 1;
    private void Awake()
    {
        Life = GetComponent<PlantDeath>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (Life.effectTrigger && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Damage(damageValue);
        }
    }
}
