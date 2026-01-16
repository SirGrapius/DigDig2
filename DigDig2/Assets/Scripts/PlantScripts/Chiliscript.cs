using System.Collections;
using UnityEngine;

public class Chiliscript : MonoBehaviour
{
    ClosestEnemy targeting;
    PlantDeath Death;
    [SerializeField] AnimationClip attackAnim;
    public Animator baseAnimator;
    [SerializeField] float maxRange = 4;
    float attackTimer;
    public bool growing;
    public float growthTimer;
    public float waterAmount = 1;
    public float waterAmountMin = 1;
    public float waterLoss = 1;
    [SerializeField] float stage1 = 30;
    [SerializeField] float stage2 = 60;
    [SerializeField] float stage3 = 90;
    [SerializeField] int damageValue = 10;
    [SerializeField] int minTargets = 5;
    [SerializeField] bool stunned = false;
    private Coroutine stunRoutine;

    [SerializeField] GameStateManager gsManager;

    private void Awake()
    {
        baseAnimator = GetComponent<Animator>();
        targeting = GetComponent<ClosestEnemy>();
        growing = true;
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    private void Start()
    {
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
                gameObject.tag = "Plant";
                growing = false;
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
        if (!growing)
        {
            GameObject[] enemiesInRange = targeting.Target(maxRange, minTargets, TargetingPrio.Close);
            if (enemiesInRange != null)
            {
                baseAnimator.SetBool("Attack", true);
                
                for (int i = 0; i < enemiesInRange.Length; i++)
                {
                    enemiesInRange[i].GetComponent<Enemy>().Damage(damageValue);
                }
            }
        }
        if (baseAnimator.GetBool("Attack"))
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackAnim.length)
            {
                Death.Decay(true);
            }
        }
    }

    public void BecomeBaby()
    {
        baseAnimator.SetBool("Child", false);
        baseAnimator.SetBool("Young", false);
        baseAnimator.SetBool("Adult", false);
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
