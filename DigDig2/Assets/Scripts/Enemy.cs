using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int health = 5;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] bool bloodlust = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] float attackCooldown = 4f; 
    private float attackTimer;
    private int currentDirection = -1;
    private Vector2 direction;

    [SerializeField] CircleCollider2D mainCollider;
    [SerializeField] Animator animator;

    [Header("Targets")]
    [SerializeField] GameObject mainTarget;
    private float mainTargetDist;
    private GameObject closestPlant;
    private float closestPlantDist;

    [Header("ChildStuff")]
    [SerializeField] Transform frontColliderTransform;
    [SerializeField] BoxCollider2D frontCollider;

    [SerializeField] List<GameObject> nearbyPlants = new List<GameObject>();
    [SerializeField] List<GameObject> frontColliderPlants = new List<GameObject>();

    void Update()
    {
        RotateFrontCollider();
        FindClosestPlant();
        CallAnimations();

        attackTimer -= Time.deltaTime;

        if (mainCollider != null)
        {
            mainCollider.radius = detectRange;
        }

        mainTargetDist = mainTarget ? Vector2.Distance(transform.position, mainTarget.transform.position) : Mathf.Infinity;

        if (mainTarget != null && mainTargetDist <= attackRange)
        {
            isAttacking = true;

            // Attack maintarget
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
        if(closestPlant != null)
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
            Debug.Log(currentDirection);
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

    void AttackClosestPlant()
    {
        if (closestPlant == null) return;

        PlantDeath plantScript = closestPlant.GetComponent<PlantDeath>();

        if (plantScript != null)
        {
            plantScript.Damage(1);
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plant") && !nearbyPlants.Contains(other.gameObject))
        {
            nearbyPlants.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            nearbyPlants.Remove(other.gameObject);
        }
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
}