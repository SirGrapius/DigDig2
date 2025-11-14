using UnityEngine;

public class CottonParticle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifespan = 5;
    [SerializeField] int damageValue = 1;
    float time;
    Vector3 origin;
    Vector3 target;
    ClosestEnemy getTarget;
    [SerializeField] float maxRange = 20;

    void Awake()
    {
        getTarget = GetComponent<ClosestEnemy>();

    }
    void Start()
    {
        target = getTarget.Target(maxRange, 1)[0].transform.position;
        origin = transform.position;
    }
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.Normalize(target - origin);
        time += Time.deltaTime;
        if (time >= lifespan)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            other.GetComponent<Enemy>().Damage(damageValue);
        }
    }
}
