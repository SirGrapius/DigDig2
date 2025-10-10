using UnityEngine;

public class ClosestEnemy : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    public GameObject Target()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Vector2 v0 = enemies[0].transform.position;
                v0.x -= transform.position.x;
                v0.y -= transform.position.y;
                Vector2 v = enemies[i].transform.position;
                v.x -= transform.position.x;
                v.y -= transform.position.y;
                if (v.sqrMagnitude < v0.sqrMagnitude)
                {
                    enemies[0] = enemies[i];
                }
            }
            return enemies[0];
        }
        return null;
    }
}
