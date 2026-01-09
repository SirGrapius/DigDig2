using System.Collections;
using UnityEngine;


public class CottonScript : MonoBehaviour
{
    [SerializeField] TargetingPrio targetType;

    ClosestEnemy targeting;
    [SerializeField] Wiggle animMoving;
    [SerializeField] GameObject Attack;
    Vector2 targetPos;
    float attackTimer;
    public float growthTimer;
    public float waterAmount = 1;
    public float waterAmountMin = 1;
    public float waterLoss = 1;
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
    [SerializeField] bool stunned = false;
    private Coroutine stunRoutine;

    [SerializeField] float maxRange = 20;

    public bool isInInventory;
    public float maxSellValue;
    public float sellValue;

    [SerializeField] GameStateManager gsManager;

    void Awake()
    {
        targeting = GetComponent<ClosestEnemy>();
        growing = true;
        sellValue = maxSellValue;
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    void Start()
    {
        Base.gameObject.tag = "GrowingPlant";
        gsManager.OnGameStateChange += OnGameStateChanged;
    }

    void Update()
    {
        if (growing)
        {
            if (waterAmount > waterAmountMin)
            {
                waterAmount -= waterLoss * Time.deltaTime;
            }
            if (waterAmount < waterAmountMin)
            {
                waterAmount = waterAmountMin;
            }
            growthTimer += Time.deltaTime * waterAmount;
            
            if (growthTimer >= stage3)
            {
                baseAnimator.SetBool("Adult", true);
                baseAnimator.SetBool("Young", true);
                baseAnimator.SetBool("Child", true);

                Base.gameObject.tag = "Plant";
                growing = false;
                animMoving.OriginPoint();
            }
            else if (growthTimer >= stage2)
            {
                baseAnimator.SetBool("Young", true);
                baseAnimator.SetBool("Child", true);
            }
            else if (growthTimer >= stage1)
            {
                baseAnimator.SetBool("Child", true);
            }
        }

        if (!growing && !isInInventory)
        {
            sellValue -= maxSellValue * 0.01f * Time.deltaTime;
            if (!ready & !stunned)
            {
                attackTimer += Time.deltaTime;
            }
            if (attackTimer >= attackFrequency)
            {
                ready = true;
                GameObject[] enemiesInRange = targeting.Target(maxRange, 1, targetType);
                if (enemiesInRange != null)
                {
                    attackTimer += Time.deltaTime;
                    myAnimator.SetBool("Attack", true);
                    if (attackTimer >= attackFrequency + attackAnim.length)
                    {
                        ready = false;
                        attackTimer -= attackFrequency + attackAnim.length;
                        targetPos = enemiesInRange[0].transform.position - transform.position;
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
                        GameObject projectile = Instantiate(Attack, transform.position, Quaternion.identity);
                        projectile.GetComponent<CottonParticle>().Spawn(targetPos);
                    }
                }
            }
        }
        if (!baseAnimator.GetBool("Adult"))
        {
            myAnimator.SetBool("Attack", false);
        }
    }
    public void BecomeBaby()
    {
        baseAnimator.SetBool("Child", false);
        baseAnimator.SetBool("Young", false);
        baseAnimator.SetBool("Adult", false);
        Base.gameObject.tag = "GrowingPlant";
        growing = true;
        growthTimer = 0;
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    public void Stun(float stunDuration)
    {
        if (stunRoutine != null)
            StopCoroutine(stunRoutine);

        stunRoutine = StartCoroutine(StunTimer(stunDuration));
    }

    private IEnumerator StunTimer(float stunDuration)
    {
        stunned = true;
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
        stunRoutine = null;
    }

    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }
}