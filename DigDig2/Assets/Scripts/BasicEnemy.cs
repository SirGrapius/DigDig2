using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int health = 5;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 1.5f;

    [Header("Targets")]
    [SerializeField] GameObject mainTarget;
    private float mainTargetDist;
    private GameObject closestPlant;
    private float closestPlantDist;

    [Header("ChildStuff")]
    [SerializeField] Transform frontCollider;   

    void Update()
    {
        RotateFrontCollider();
        FindClosestPlant();

        mainTargetDist = Mathf.Infinity;
        if(mainTarget != null)
        {
            mainTargetDist = Vector2.Distance(transform.position, mainTarget.transform.position);
        }

        if(mainTarget != null && mainTargetDist <= attackRange)
        {

        }
        else if(closestPlant != null && closestPlantDist <= attackRange)
        {

        }
        else if(closestPlant != null && closestPlantDist <= detectRange && closestPlantDist < mainTargetDist) 
        {
            MoveTowardsTarget(closestPlant.transform.position);
        }
        else
        {
            if(mainTarget != null)
            {
                MoveTowardsTarget(mainTarget.transform.position);
            }
        }
    }

    void FindClosestPlant()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        closestPlant = null;
        closestPlantDist = Mathf.Infinity;

        foreach (var plant in plants)
        {
            if(plants == null) continue;
            float dist = Vector2.Distance(transform.position, plant.transform.position);
            if(dist < closestPlantDist)
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
        if(frontCollider != null || mainTarget != null)
        {
            Vector2 direction = (mainTarget.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            frontCollider.rotation = Quaternion.Euler(0,0,angle - 90f);
         }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
