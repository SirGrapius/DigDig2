using UnityEngine;

public class EnemyChildColliderRelay : MonoBehaviour
{
    [SerializeField] private Enemy parentEnemy; // reference to your main enemy script

    private void OnTriggerEnter2D(Collider2D other)
    {
        parentEnemy?.OnChildTriggerEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        parentEnemy?.OnChildTriggerExit(other);
    }
}
