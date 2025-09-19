using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int health = 5;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] bool bloodlust = false;

    [SerializeField] CircleCollider2D mainCollider;

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

        if (mainCollider != null) { mainCollider.radius = detectRange; }
        mainTargetDist = mainTarget ? Vector2.Distance(transform.position, mainTarget.transform.position) : Mathf.Infinity;

        if (mainTarget != null && mainTargetDist <= attackRange)
        {
            // Attack maintarget
        }
        else if (closestPlant != null && closestPlantDist <= attackRange)
        {
            AttackClosestPlant();
        }
        else if (closestPlant != null && closestPlantDist <= detectRange && closestPlantDist < mainTargetDist)
        {
            MoveTowardsTarget(closestPlant.transform.position);
        }
        else
        {
            if (mainTarget != null)
            {
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