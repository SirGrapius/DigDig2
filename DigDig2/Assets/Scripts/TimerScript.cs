using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Vector3 start, end, current;
    [SerializeField] GameObject back;
    [SerializeField] float timedPos;
    [SerializeField] float timeMulti = 0.1f;
    [SerializeField] float dayTime = 300;
    [SerializeField] Vector3 scale;
    [SerializeField] Sprite[] dayNightSprites;
    bool night;
    bool fliped;
    bool flipping;
    [SerializeField]float flipspeed;

    void Start()
    {
        scale = Vector3.one;
        current = back.transform.position + start;
        current.z = 90;
        timeMulti = end.x / dayTime;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!flipping) 
        {
            timedPos += Time.deltaTime * timeMulti;
        }
        current.x = ((start.x + (timedPos * 2)) * scale.x) + back.transform.position.x;
        current.y = back.transform.position.y;
        transform.position = current;

        if(timedPos >= 1)
        {
            night = !night;
            flipping = true;
        }
        if (flipping)
        {
            timedPos -= Time.deltaTime * end.x * (flipspeed / 2);
        }
        if (timedPos < 0)
        {
            timedPos = 0;
            flipping = false;
        }
        if (night)
        {
            if (!fliped)
            {
                scale.x -= Time.deltaTime * flipspeed;
            }
            if (scale.x < 0)
            {
                scale.x = 0;
                back.GetComponent<SpriteRenderer>().sprite = dayNightSprites[1];
                fliped = true;
            }
            if (fliped)
            {
                scale.x += Time.deltaTime * flipspeed;
            }
            if (scale.x > 1)
            {
                scale.x = 1;
            }
        }
        else
        {
            if (fliped)
            {
                scale.x -= Time.deltaTime * flipspeed;
            }
            if (scale.x < 0)
            {
                scale.x = 0;
                back.GetComponent<SpriteRenderer>().sprite = dayNightSprites[0];
                fliped = false;
            }
            if (fliped)
            {
                scale.x += Time.deltaTime * flipspeed;
            }
            if (scale.x > 1)
            {
                scale.x = 1;
            }
        }
        back.transform.localScale = scale;
    }
}
