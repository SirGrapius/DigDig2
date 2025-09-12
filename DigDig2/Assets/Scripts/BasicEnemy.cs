using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] int health = 5;
    [SerializeField] int movementSpeed = 1;
    [SerializeField] GameObject mainTarget;
    [SerializeField] GameObject closestTarget;
    [SerializeField] GameObject[] plants;
    private Rigidbody2D rb;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isAttacking)
        {
            AttackPhase();
        }
        else
        {
            MovePhase();
        }
    }

    void MovePhase()
    {
        if (mainTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, mainTarget.transform.position, movementSpeed * Time.deltaTime);

            transform.up = mainTarget.transform.position - transform.position;

            plants = GameObject.FindGameObjectsWithTag("plant");


        }
    }

    void AttackPhase()
    {

    }




    // Walk phase, walk towards barn and detect plants infront of it and around, if detected enter attack phase by switching attacking bool on.

    // In attack phase, check if there are plants within attack range infront, if there is attack, if theres not,
    // move towards closest plant within detect range, and if there is no plants within detect range, switch bool is attacking off.
    // After eating a plant, switch the bool "Bloodlust" to true, where it only detects plants infront of it now in AttackPhase and MovePhase

}
