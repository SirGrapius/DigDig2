using UnityEngine;

public class CrowProjectile : MonoBehaviour
{
    public GameObject targetplant;
    private float movementSpeed = 3f;
    private float attackRange = 1f;
    private AnyPlant plantScript;
    private PlantDeath plantDeathScript;

    [SerializeField] Animator animator;

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
            animator.SetTrigger("Arrived");
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

    void FinishedPlant()
    {
        plantDeathScript.Decay(false);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
