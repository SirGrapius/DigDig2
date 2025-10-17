using UnityEngine;

public class CottonScript : MonoBehaviour
{
    ClosestEnemy targeting;
    [SerializeField] Wiggle animMoving;
    [SerializeField] GameObject Attack;
    Vector2 targetPos;
    float time;
    [SerializeField] float attackFrequency;
    [SerializeField] Animator myAnimator;
    [SerializeField] public Animator baseAnimator;
    [SerializeField] AnimationClip attackAnim;
    [SerializeField] AnimationClip growAnim;
    [SerializeField] Transform Base;
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
        if (growing && time >= growAnim.length)
        {
            baseAnimator.SetBool("Adult", true);
            time = 0;
            growing = false;
            animMoving.OriginPoint();
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
                        switch (targetPos.x)
                        {
                            case > 0:
                                Base.localScale = new Vector3(-1, 1, 1);
                                break;
                            case < 0:
                                Base.localScale = Vector3.one;
                                break;

                        }
                        myAnimator.SetBool("Attack", false);
                        Instantiate(Attack, transform.position, Quaternion.identity);
                    }
                }
            }
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg);
        }
    }
}