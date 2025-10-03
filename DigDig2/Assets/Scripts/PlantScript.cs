using UnityEngine;

public class PlantScript : MonoBehaviour
{
    [SerializeField] GameObject daddy;
    Vector2 targetPos;
    [SerializeField] GameObject[] enemies;
    float time;
    [SerializeField] float attackFrequency;
    [SerializeField] int hp;
    [SerializeField] Animator myAnimator;
    [SerializeField] AnimationClip attackAnim;
    bool ready;

    void Update()
    {
        
        if (!ready)
        {
            time += Time.deltaTime;
        }
        if (time >= attackFrequency)
        {
            ready = true;
            if (Target() != null)
            {
                time += Time.deltaTime;
                myAnimator.SetBool("Attack", true);
                if (time >= attackFrequency + attackAnim.length)
                {
                    ready = false;
                    time -= attackFrequency + attackAnim.length;
                    targetPos = Target().transform.position;
                    targetPos.x = targetPos.x - transform.position.x;
                    targetPos.y = targetPos.y - transform.position.y;
                    myAnimator.SetBool("Attack", false);
                }
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg);
    }

    GameObject Target()
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
    public void Damage(int damageValue)
    {
        hp -= damageValue;
        if (hp <= 0)
        {
            Decay();
        }
    }

    void Decay()
    {
        Destroy(daddy); 
    }
}