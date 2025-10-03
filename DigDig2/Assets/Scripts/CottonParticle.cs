using UnityEngine;

public class CottonParticle : MonoBehaviour
{
    [SerializeField] float Speed;
    Vector2 target;
    ClosestEnemy getTarget;
    void Awake()
    {
        getTarget = GetComponent<ClosestEnemy>();

    }
    void Start()
    {
        target = getTarget.Target().transform.position;
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
    }
}
