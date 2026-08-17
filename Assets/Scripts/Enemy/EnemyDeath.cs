using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
   void ExplosionSound()
    {

    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
