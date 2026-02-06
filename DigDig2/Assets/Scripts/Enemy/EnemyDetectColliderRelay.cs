using UnityEngine;

public class EnemyDetectColliderRelay : MonoBehaviour
{
    [SerializeField] private Enemy parentEnemy; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        parentEnemy?.OnChildTriggerEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        parentEnemy?.OnChildTriggerExit(other);
    }
}
