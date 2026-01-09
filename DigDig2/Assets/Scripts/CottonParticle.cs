using UnityEngine;

public class CottonParticle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifespan = 5;
    [SerializeField] int damageValue = 1;
    float time;
    Vector3 origin;
    Vector3 target;

    [SerializeField] GameStateManager gsManager;

    void Awake()
    {
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    public void Spawn(Vector3 attackTarget)
    {
        target = attackTarget;
        origin = transform.position;
        gsManager.OnGameStateChange += OnGameStateChanged;
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

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }
}
