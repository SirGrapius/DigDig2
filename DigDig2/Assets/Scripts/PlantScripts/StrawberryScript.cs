using UnityEngine;

public class StrawberryScript : MonoBehaviour
{
    [SerializeField] TargetingPrio targetType;

    [SerializeField] GameObject Attack;
    [SerializeField] ClosestEnemy targeting;
    [SerializeField] float attackInterval;
    float attackSpeed = 1;
    [SerializeField] float attackTimer;
    float maxRange = 10;
    float aoe = 1;
    [SerializeField] int aoeDamage = 1;
    bool ready;

    void Awake()
    {
        targeting = GetComponent<ClosestEnemy>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (!ready)
        {
            attackTimer += Time.deltaTime * attackSpeed;
        }
        if (attackTimer > attackInterval)
        {
            ready = true;
            GameObject[] enemiesInRange = targeting.Target(maxRange, 1, targetType);
            if (enemiesInRange != null)
            {
                ready = false;
                attackTimer -= attackInterval;
                GameObject projectile = Instantiate(Attack, enemiesInRange[0].transform.position, Quaternion.identity);
                GameObject[] enemiesInAoe = projectile.GetComponent<ClosestEnemy>().Target(aoe, 1, targetType);
                for (int i = 1; i < enemiesInAoe.Length; i++)
                {
                    enemiesInRange[i].GetComponent<Enemy>().Damage(aoeDamage);
                }
            }
        }
    }
}
