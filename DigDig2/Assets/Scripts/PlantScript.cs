using UnityEngine;

public class PlantScript : MonoBehaviour
{
    Vector2 targetPos;
    [SerializeField] GameObject[] enemies;
    float time;
    [SerializeField] float attackFrequency;
        
    void Update()
    {
        time += Time.deltaTime;
        if (time >= attackFrequency)
        {
            time = 0;
            targetPos = Attack().transform.position;
            targetPos.x = targetPos.x - transform.position.x;
            targetPos.y = targetPos.y - transform.position.y;
        }
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg);
    }

    GameObject Attack()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
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
}