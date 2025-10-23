using UnityEngine;

public class CottonScript : MonoBehaviour
{
    ClosestEnemy targeting;
    [SerializeField] Wiggle animMoving;
    [SerializeField] GameObject Attack;
    Vector2 targetPos;
    public float growthTimer;
    float attackTimer;
    [SerializeField] float stage1 = 30;
    [SerializeField] float stage2 = 60;
    [SerializeField] float stage3 = 90;
    [SerializeField] float attackFrequency;
    [SerializeField] Animator myAnimator;
    public Animator baseAnimator;
    [SerializeField] AnimationClip attackAnim;
    [SerializeField] Transform Base;
    public bool ready;
    public bool growing;
    public float waterAmount = 1;
    public float waterLoss;
    [SerializeField] float maxRange = 20;

    void Awake()
    {
        targeting = GetComponent<ClosestEnemy>();
        growing = true;
    }

    void Update()
    {
        if (growing)
        {
            growthTimer += Time.deltaTime * waterAmount;
            if (waterAmount > 1)
            {
                waterAmount -= waterLoss * Time.deltaTime;
            }
            if (growthTimer >= stage3)
            {
                baseAnimator.SetBool("Adult", true);
                growthTimer = 0;
                growing = false;
                animMoving.OriginPoint();
            }
            else if (growthTimer >= stage2)
            {
                baseAnimator.SetBool("Teen", true);
            }
            else if (growthTimer >= stage1)
            {
                baseAnimator.SetBool("Young", true);
            }
        }

        if (!growing)
        {
            if (!ready)
            {
                attackTimer += Time.deltaTime;
            }
            if (attackTimer >= attackFrequency)
            {
                ready = true;
                if (targeting.Target(maxRange) != null)
                {
                    attackTimer += Time.deltaTime;
                    myAnimator.SetBool("Attack", true);
                    if (attackTimer >= attackFrequency + attackAnim.length)
                    {
                        ready = false;
                        attackTimer -= attackFrequency + attackAnim.length;
                        targetPos = targeting.Target(maxRange).transform.position;
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