using UnityEngine;

public class ClosestEnemy : MonoBehaviour
{
    GameObject[] enemies;
    
    public GameObject[] Target(float maxRange, int minTargets)
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemiesInRange = enemies;
        int counter = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 centered = enemies[i].transform.position - transform.position;
            if (centered.sqrMagnitude < maxRange * maxRange)
            {
                enemiesInRange[counter] = enemies[i];
                counter++;
            }
        }
        enemies = enemiesInRange[..counter];
        if (enemies.Length < minTargets)
        {
            return null;
        }
        GameObject temp;
        for (int i = 0; i < enemies.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                Vector3 v0 = enemies[j].transform.position - transform.position;
                Vector3 v1 = enemies[i].transform.position - transform.position;
                if (v0.sqrMagnitude > v1.sqrMagnitude)
                {
                    temp = enemies[j];
                    enemies[j] = enemies[i];
                    enemies[i] = temp;
                }
            }
        }
        return enemies;
    }
}
