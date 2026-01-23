using UnityEngine;

public class StrawberryParticle : MonoBehaviour
{
    float lifetime = 1;
    float timer;
    void Awake()
    {
        timer = 0;   
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
