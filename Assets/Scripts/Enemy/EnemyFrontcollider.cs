using UnityEngine;

public class EnemyFrontcollider : MonoBehaviour
{
    Enemy parent;
    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        parent.GetFrontColliderEntries(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        parent.GetFrontColliderExits(other);
    }
}
