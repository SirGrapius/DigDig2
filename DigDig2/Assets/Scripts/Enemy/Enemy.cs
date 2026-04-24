using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int hp = 5;
    [SerializeField] int attackDamage = 1;
    [SerializeField] int bloodlustCharges = 0;
    [SerializeField] int neededBloodlustCharges = 2;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float attackCooldown = 3f;
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] bool bloodlust = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isFlying = false;

    int currentDirection = -1;
    float closestPlantDist;
    float mainTargetDist;
    float attackTimer;
    
    Vector2 direction;

    private Coroutine damageFlashRoutine;


    [Header("References")]
    [SerializeField] CircleCollider2D detectCollider;
    [SerializeField] Animator animator;
    [SerializeField] Transform frontColliderTransform;
    [SerializeField] BoxCollider2D frontCollider;
    [SerializeField] SpriteRenderer spriteRenderer;

    BoxCollider2D mainTargetCollider;
    RoundManager roundManagerScript;
    GameStateManager gsManager;
    GameObject closestPlant;
    GameObject mainTarget;
    GameObject minimap;


    [Header("Lists")]
    [SerializeField] List<GameObject> nearbyPlants = new List<GameObject>();
    [SerializeField] List<GameObject> frontColliderPlants = new List<GameObject>();

    private void Awake()
    {
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    private void Start()
    {
        mainTarget = GameObject.FindGameObjectWithTag("MainTarget");
        mainTargetCollider = mainTarget.GetComponent<BoxCollider2D>();

        if (GameObject.FindGameObjectWithTag("Minimap") != null)
        {
            Minimap manager = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Minimap>();
            manager.RegisterEnemy(transform);
        }
        
        roundManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoundManager>(); 
        gsManager.OnGameStateChange += OnGameStateChanged;
    }

    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }

    void Update()
    {
        RotateFrontCollider();
        FindClosestPlant();
        CallAnimations();

        attackTimer -= Time.deltaTime;

        if (detectCollider != null) 
        {
            detectCollider.radius = detectRange; // Set collider on child object to the size of this scripts variable
        }

        Vector2 closestPoint = mainTargetCollider.ClosestPoint(transform.position); // Calculates the closest point on the houses collider relative to the enemy
        float mainTargetDist = Vector2.Distance(transform.position, closestPoint);  // Calculates the distance between the enemy and the house colliders closest point

        if (mainTarget != null && mainTargetDist <= attackRange || closestPlant != null && closestPlantDist <= attackRange)
        {
            isAttacking = true;

            if (attackTimer <= 0f)
            {
                animator.SetTrigger("Attack");
                attackTimer = attackCooldown;
            }
        }
        else if (closestPlant != null && closestPlantDist <= detectRange && closestPlantDist < mainTargetDist) 
        {
            isAttacking = false;

            MoveTowardsTarget(closestPlant.transform.position);
        }
        else
        {
            if (mainTarget != null)
            {
                isAttacking = false;

                MoveTowardsTarget(mainTarget.transform.position);
            }
        }
    }

    #region DetectionLogic
    void RotateFrontCollider() // Rotates the front collider to always face the house
    {
        if (frontColliderTransform != null && mainTarget != null)
        {
            Vector2 direction = (mainTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            frontColliderTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    void FindClosestPlant() // Finds the closest plant out of those in the nearbyPlants list
    {
        closestPlant = null;
        closestPlantDist = Mathf.Infinity;

        List<GameObject> plantsToSearch = bloodlust ? frontColliderPlants : nearbyPlants;

        for (int i = plantsToSearch.Count - 1; i >= 0; i--)
        {
            GameObject plant = plantsToSearch[i];
            if (plant == null || !plant.activeInHierarchy)
            {
                plantsToSearch.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(transform.position, plant.transform.position);
            if (dist < closestPlantDist)
            {
                closestPlant = plant;
                closestPlantDist = dist;
            }
        }
    }

    public void OnChildTriggerEnter(Collider2D other) // Adds any plants within a childs collider to a list
    {
        if (isFlying && other.GetComponent<PotatoScript>() != null || isFlying && other.GetComponent<ChilliScript>() != null)
            return;

        if (other.CompareTag("Plant") && !nearbyPlants.Contains(other.gameObject))
            nearbyPlants.Add(other.gameObject);
    }

    public void OnChildTriggerExit(Collider2D other)
    {
        if (isFlying && other.GetComponent<PotatoScript>() != null || isFlying && other.GetComponent<ChilliScript>() != null)
            return;

        if (other.CompareTag("Plant"))
            nearbyPlants.Remove(other.gameObject);
    }

    public void GetFrontColliderEntries(Collider2D other) // Adds any plants within another childs collider to a list using that childs script aswell as this
    {
        if (isFlying && other.GetComponent<PotatoScript>() != null || isFlying && other.GetComponent<ChilliScript>() != null)
            return;

        if (other.CompareTag("Plant") && !frontColliderPlants.Contains(other.gameObject))
        {
            frontColliderPlants.Add(other.gameObject);
        }
    }

    public void GetFrontColliderExits(Collider2D other)
    {
        if (isFlying && other.GetComponent<PotatoScript>() != null || isFlying && other.GetComponent<ChilliScript>() != null)
            return;

        if (other.CompareTag("Plant"))
        {
            frontColliderPlants.Remove(other.gameObject);
        }
    }
    #endregion DetectionLogic

    void CallAnimations()
    {
        if(closestPlant != null && closestPlantDist <= detectRange)
        {
            direction = (closestPlant.transform.position - transform.position).normalized;
        }
        else
        {
            direction = (mainTarget.transform.position - transform.position).normalized;
        }

        int newDirection;

        if (isAttacking == false && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            newDirection = direction.x > 0 ? 1 : 3; 
        }
        else if (isAttacking == false)
        {
            newDirection = direction.y > 0 ? 0 : 2;
        }
        else
        {
            newDirection = 4;
        }

        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;
            animator.SetInteger("Direction", currentDirection);
            animator.SetTrigger("ChangeDirection");
        }

    }

    void MoveTowardsTarget(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
    }

    #region AttackLogic

    void Attack()
    {
        if (mainTarget != null && mainTargetDist <= attackRange) // If the house colliders closest point is within attack range, attack the house
        {
             AttackMainTarget();
        }
        else if (closestPlant != null && closestPlantDist <= attackRange) // Else if the closest plant is within attack range, attack that plant
        {
             AttackClosestPlant(); 
        }
    }

    void AttackMainTarget()
    {
        roundManagerScript.houseHealth -= attackDamage;
    }
    
    void AttackClosestPlant()
    {
        if (closestPlant == null) return;

        PlantDeath plantScript = closestPlant.GetComponent<PlantDeath>();

        if (plantScript != null && closestPlant.GetComponent<PotatoScript>() != null)
        {
            plantScript.Damage(attackDamage);
            if (closestPlant.GetComponent<PotatoScript>() != null)
            {
                Damage(closestPlant.GetComponent<PotatoScript>().damageValue);
            }
        }
        else if (plantScript != null)
        {
            plantScript.Damage(attackDamage);
        }
        else
        {
            Debug.LogWarning("No PlantDeath script found");
        }

        if (plantScript.decaying)
        {
            if (bloodlust) return;

            bloodlustCharges += 1;

            if (bloodlustCharges >= neededBloodlustCharges)
            {
                bloodlust = true;
            }
        }
    }
    #endregion AttackLogic

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    public void Damage(int damageValue)
    {
        hp -= damageValue;

        DamageFlash();

        if (hp <= 0)
        {
            Die();
        }
    }

    void DamageFlash()
    {
        if (damageFlashRoutine != null)
        {
            StopCoroutine(damageFlashRoutine);
        }

        damageFlashRoutine = StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        Color takeDamageColor = new Color32(244, 61, 61, 255);

        spriteRenderer.color = takeDamageColor;

        float duration = 1.2f;
        float t = 0;

        while(t < duration)
        {
            t += Time.deltaTime;

            spriteRenderer.color = Color.Lerp(takeDamageColor, originalColor, t / duration);

            yield return null; 
        }

        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}