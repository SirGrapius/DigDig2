using UnityEngine;

public class Wiggle : MonoBehaviour
{
    Vector2 origin;
    [SerializeField] Vector2 WigglePos;
    [SerializeField] AnimationClip wiggleTime;
    float time;
    private void Awake()
    {
        origin = transform.position;
    }
    void Update()
    {
        time += Time.deltaTime * 0.1f;
        if (time >= wiggleTime.length/2)
        {
            transform.position = WigglePos + origin;
        }
        if (time >= wiggleTime.length)
        {
            transform.position = origin;
            time -= wiggleTime.length;
        }
    }
}
