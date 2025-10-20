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
                Vector3 v0 = enemies[0].transform.position;
                v0 -= transform.position;
                Vector3 v = enemies[i].transform.position;
                v -= transform.position;
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
