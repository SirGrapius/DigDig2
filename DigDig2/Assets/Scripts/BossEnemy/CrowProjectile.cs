using UnityEngine;

public class CrowProjectile : MonoBehaviour
{
    public GameObject targetplant;
    private float movementSpeed = 3f;
    private float attackRange = 1f;
    private float eatTime = 5f;
    private float eatTimer = 0f;
    private AnyPlant plantScript;
    private PlantDeath plantDeathScript;

    private void Start()
    {
        plantDeathScript = targetplant.GetComponent<PlantDeath>();
        plantScript = targetplant.GetComponent<AnyPlant>();
    }

    void Update()
    {
        Collider2D targetPlantCollider = targetplant.GetComponent<Collider2D>();
        Vector2 plantClosestPoint = targetPlantCollider.ClosestPoint(transform.position);

        if (Vector2.Distance(transform.position, plantClosestPoint) < attackRange && plantScript != null && plantScript.IsGrowing()) 
        {
            if (eatTimer >= eatTime) 
            {
                plantDeathScript.Decay(false);
            }
            else
            {
                eatTimer += Time.deltaTime;
            }
        }
        else if (plantScript != null && plantScript.IsGrowing())
        {
            transform.position = Vector2.MoveTowards(transform.position, targetplant.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            Death();
        }
            
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
