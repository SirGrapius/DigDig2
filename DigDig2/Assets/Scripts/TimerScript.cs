using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Vector3 start, end, current;
    [SerializeField] GameObject back;
    float time;
    [SerializeField] float timeMulti = 0.1f;

    void Start()
    {
        current = back.transform.position + start;
        current.z = 90;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * timeMulti;
        current.x = start.x + back.transform.position.x + time;
        current.y = back.transform.position.y;
        transform.position = current;
        if( current.x > end.x + back.transform.position.x)
        {
            current.x = start.x + back.transform.position.x;
            time = 0;
        }
    }
}
