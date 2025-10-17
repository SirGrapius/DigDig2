using UnityEngine;

public class CottonParticle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifespan = 5;
    float time;
    Vector3 origin;
    Vector3 target;
    ClosestEnemy getTarget;
    void Awake()
    {
        getTarget = GetComponent<ClosestEnemy>();

    }
    void Start()
    {
        target = getTarget.Target().transform.position;
        origin = transform.position;
    }
    void Update()
    {
        transform.position += Vector3.MoveTowards(origin, target, speed * Time.deltaTime) - origin;
        time += Time.deltaTime;
        if (time >= lifespan)
        {
            Destroy(gameObject);
        }
    }
}
