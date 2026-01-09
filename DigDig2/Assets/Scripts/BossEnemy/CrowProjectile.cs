using UnityEngine;

public class CrowProjectile : MonoBehaviour
{
    public Vector2 targetplant;
    private float movementSpeed = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        if (targetplant != null) { transform.position = Vector2.MoveTowards(transform.position, targetplant, movementSpeed * Time.deltaTime); }
    }
}
