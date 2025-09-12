using UnityEngine;

public class PlantScript : MonoBehaviour
{
    Vector2 targetPos;
    [SerializeField] GameObject[] enemies;
    float time;
    [SerializeField] float attackFrequency;
        
    void Update()
    {
        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.x = targetPos.x - transform.position.x;
        targetPos.y = targetPos.y - transform.position.y;
        transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg);
        time += Time.deltaTime;
        if (time >= attackFrequency)
        {
            time = 0;
            Attack();
        }
    }
    void Attack()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector2 v = enemies[i].transform.position;
            v.x -= transform.position.x;
            v.y -= transform.position.y;
            v.x = Mathf.Abs(v.x);
            v.y = Mathf.Abs(v.y);
            if (v.sqrMagnitude < enemies[i].transform.position.sqrMagnitude)
            {

            }
        }
    }
}