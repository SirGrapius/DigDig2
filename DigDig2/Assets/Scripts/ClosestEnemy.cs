using UnityEngine;

public enum TargetingPrio
{
    Close,
    Far,
    Strong,
    Weak,
    NearHouse,
    Aerial,
    Total
}

public class ClosestEnemy : MonoBehaviour
{
    GameObject[] enemies;
    
    public GameObject[] Target(float maxRange, int minTargets, TargetingPrio priority)
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
                Vector3 vj = enemies[j].transform.position - transform.position;
                Vector3 vi = enemies[i].transform.position - transform.position;
                if (priority == TargetingPrio.NearHouse)
                {
                    vj = enemies[j].transform.position;
                    vi = enemies[i].transform.position;
                }
                switch (priority)
                {
                    case TargetingPrio.Close:
                        if (vj.sqrMagnitude > vi.sqrMagnitude)
                        {
                            temp = enemies[j];
                            enemies[j] = enemies[i];
                            enemies[i] = temp;
                        }
                        break;
                    case TargetingPrio.Far:
                        if (vj.sqrMagnitude < vi.sqrMagnitude)
                        {
                            temp = enemies[j];
                            enemies[j] = enemies[i];
                            enemies[i] = temp;
                        }
                        break;
                    case TargetingPrio.Strong:
                        if (enemies[j].GetComponent<Enemy>().hp < enemies[i].GetComponent<Enemy>().hp)
                        {
                            temp = enemies[j];
                            enemies[j] = enemies[i];
                            enemies[i] = temp;
                        }
                        else if (vj.sqrMagnitude > vi.sqrMagnitude &&
                            enemies[j].GetComponent<Enemy>().hp == enemies[i].GetComponent<Enemy>().hp
                            )
                        {
                            temp = enemies[j];
                            enemies[j] = enemies[i];
                            enemies[i] = temp;
                        }
                        break;
                    case TargetingPrio.Weak:
                        if (enemies[j].GetComponent<Enemy>().hp > enemies[i].GetComponent<Enemy>().hp)
                        {
                            temp = enemies[j];
                            enemies[j] = enemies[i];
                            enemies[i] = temp;
                        }
                        else if (vj.sqrMagnitude > vi.sqrMagnitude && 
                            enemies[j].GetComponent<Enemy>().hp == enemies[i].GetComponent<Enemy>().hp)
                        {
                            temp = enemies[j];
                            enemies[j] = enemies[i];
                            enemies[i] = temp;
                        }
                        break;
                    case TargetingPrio.Aerial:
                        break;
                }
            }
        }
        return enemies;
    }
}
