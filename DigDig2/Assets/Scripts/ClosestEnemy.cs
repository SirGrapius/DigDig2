using UnityEngine;

public class ClosestEnemy : MonoBehaviour
{
    GameObject[] enemies;
    public GameObject Target(float maxRange)
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 v0;
                if (enemies[0] != null)
                {
                    v0 = enemies[0].transform.position - transform.position;
                }
                else
                {
                    v0 = enemies[i].transform.position - transform.position;
                }
                Vector3 v = enemies[i].transform.position - transform.position;
                if (v.sqrMagnitude > maxRange * maxRange)
                {
                    enemies[i] = null;
                }
                else if (v.sqrMagnitude < v0.sqrMagnitude && enemies[0] != null)
                {
                    enemies[0] = enemies[i];
                }
                else if (v.sqrMagnitude == v0.sqrMagnitude)
                {
                    enemies[0] = enemies[i];
                }
            }
            return enemies[0];
        }
        return null;
    }
}
