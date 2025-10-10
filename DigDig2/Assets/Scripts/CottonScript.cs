using UnityEngine;

public class CottonScript : MonoBehaviour
{
    ClosestEnemy targeting;
    Vector2 targetPos;
    float time;
    [SerializeField] float attackFrequency;
    [SerializeField] Animator myAnimator;
    [SerializeField] public Animator baseAnimator;
    [SerializeField] AnimationClip attackAnim;
    [SerializeField] AnimationClip growAnim;
    bool ready;
    bool growing;

    void Awake()
    {
        targeting = GetComponent<ClosestEnemy>();
        growing = true;
    }

    void Update()
    {
        if (growing)
        {
            time += Time.deltaTime;
        }
        if (time >= growAnim.length)
        {
            baseAnimator.SetBool("Adult", true);
            time = 0;
            growing = false;
        }

        if (baseAnimator.GetBool("Adult"))
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
                        targetPos.x -= transform.position.x;
                        targetPos.y -= transform.position.y;
                        myAnimator.SetBool("Attack", false);
                    }
                }
            }
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg);
        }
    }
}