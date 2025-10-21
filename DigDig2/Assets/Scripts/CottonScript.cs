using UnityEngine;

public class CottonScript : MonoBehaviour
{
    ClosestEnemy targeting;
    [SerializeField] Wiggle animMoving;
    [SerializeField] GameObject Attack;
    Vector2 targetPos;
    public float time;
    [SerializeField] float stage1 = 30;
    [SerializeField] float stage2 = 60;
    [SerializeField] float stage3 = 90;
    [SerializeField] float attackFrequency;
    [SerializeField] Animator myAnimator;
    [SerializeField] public Animator baseAnimator;
    [SerializeField] AnimationClip attackAnim;
    [SerializeField] Transform Base;
    public bool ready;
    public bool growing;

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
        if (growing && time >= stage1)
        {
            baseAnimator.SetBool("Young", true);    
        }
        if (growing && time >= stage2)
        {
            baseAnimator.SetBool("Teen", true);
        }
        if (growing && time >= stage3)
        {
            baseAnimator.SetBool("Adult", true);
            time = 90;
            growing = false;
            animMoving.OriginPoint();
        }

        if (!growing)
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