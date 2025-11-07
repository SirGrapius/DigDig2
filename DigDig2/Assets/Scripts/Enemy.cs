using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int hp = 5;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] bool bloodlust = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] float attackCooldown = 3f;
    [SerializeField] int attackDamage = 1;
    private float attackTimer;
    private int currentDirection = -1;
    private Vector2 direction;

    [Header("Components")]
    [SerializeField] CircleCollider2D detectCollider;
    [SerializeField] Animator animator;
    [SerializeField] Transform frontColliderTransform;
    [SerializeField] BoxCollider2D frontCollider;

    BoxCollider2D mainTargetCollider;

    [Header("Targets")]
    private GameObject mainTarget;
    private float mainTargetDist;
    private GameObject closestPlant;
    private float closestPlantDist;

    [Header("ChildStuff")]
    [SerializeField] List<GameObject> nearbyPlants = new List<GameObject>();
    [SerializeField] List<GameObject> frontColliderPlants = new List<GameObject>();

    private void Start()
    {
        mainTarget = GameObject.FindGameObjectWithTag("MainTarget");
        mainTargetCollider = mainTarget.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        RotateFrontCollider();
        FindClosestPlant();
        CallAnimations();

        attackTimer -= Time.deltaTime;

        if (detectCollider != null)
        {
            detectCollider.radius = detectRange;
        }

        Vector2 closestPoint = mainTargetCollider.ClosestPoint(transform.position);
        float mainTargetDist = Vector2.Distance(transform.position, closestPoint);

        if (mainTarget != null && mainTargetDist <= attackRange)
        {
            isAttacking = true;

            if (attackTimer <= 0f)
            {
                AttackMainTarget();
                attackTimer = attackCooldown;
            }
        }
        else if (closestPlant != null && closestPlantDist <= attackRange)
        {
            isAttacking = true;

            if (attackTimer <= 0f)
            {
                AttackClosestPlant();
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

    void FindClosestPlant()
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

        int newDirectionIndex;

        if (isAttacking == false && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            newDirectionIndex = direction.x > 0 ? 1 : 3; 
        }
        else if(isAttacking == false)
        {
            newDirectionIndex = direction.y > 0 ? 0 : 2;
        }
        else
        {
            newDirectionIndex = 4;
        }

        if (newDirectionIndex != currentDirection)
        {
            currentDirection = newDirectionIndex;
            animator.SetInteger("Direction", currentDirection);
        }
    }

    void MoveTowardsTarget(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
    }

    void RotateFrontCollider()
    {
        if (frontColliderTransform != null && mainTarget != null)
        {
            Vector2 direction = (mainTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            frontColliderTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    void AttackMainTarget()
    {
        // Replace BarnScript with the correct script name for the barn/base/mainTarget, and then remove the /* and */

        /*
        if (mainTarget == null) return;

        BarnScript mainTargetScript = mainTarget.GetComponent<BarnScript>();

        if (mainTargetScript != null)
        {
            mainTargetScript.Damage(1);
        }
        else
        {
            Debug.LogWarning("No BarnScript found on mainTarget");
        }
        */ 
    }


    void AttackClosestPlant()
    {
        if (closestPlant == null) return;

        PlantDeath plantScript = closestPlant.GetComponent<PlantDeath>();

        if (plantScript != null && closestPlant.GetComponent<PotatoScript>() != null)
        {
            plantScript.Damage(attackDamage);

            Damage(closestPlant.GetComponent<PotatoScript>().damageValue);
        }
        else if (plantScript != null)
        {
            plantScript.Damage(attackDamage);
        }
        else
        {
            Debug.LogWarning("No PlantDeath script found");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    public void OnChildTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("Plant") && !nearbyPlants.Contains(other.gameObject))
            nearbyPlants.Add(other.gameObject);
    }

    public void OnChildTriggerExit(Collider2D other)
    {
        if (other.CompareTag("Plant"))
            nearbyPlants.Remove(other.gameObject);
    }

    public void GetFrontColliderEntries(Collider2D other)
    {
        if (other.CompareTag("Plant") && !frontColliderPlants.Contains(other.gameObject))
        {
            frontColliderPlants.Add(other.gameObject);
        }
    }

    public void GetFrontColliderExits(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            frontColliderPlants.Remove(other.gameObject);
        }
    }

    public void Damage(int damageValue)
    {
        hp -= damageValue;
        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}