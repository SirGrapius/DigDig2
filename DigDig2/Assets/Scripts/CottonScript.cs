using UnityEngine;

public class CottonScript : MonoBehaviour
{
    ClosestEnemy targeting;
    Vector2 targetPos;
    float time;
    [SerializeField] float attackFrequency;
    [SerializeField] Animator myAnimator;
    [SerializeField] AnimationClip attackAnim;
    bool ready;

    void Awake()
    {
        targeting = GetComponent<ClosestEnemy>();
    }

    void Update()
    {
        if (!ready)
        {
            time += Time.deltaTime;
        }
        if (time >= attackFrequency)
        {
            ready = true;
            if (targeting.Target() != null)
            {
                time += Time.deltaTime;
                myAnimator.SetBool("Attack", true);
                if (time >= attackFrequency + attackAnim.length)
                {
                    ready = false;
                    time -= attackFrequency + attackAnim.length;
                    targetPos = targeting.Target().transform.position;
                    targetPos.x = targetPos.x - transform.position.x;
                    targetPos.y = targetPos.y - transform.position.y;
                    myAnimator.SetBool("Attack", false);
                }
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg);
    }
}