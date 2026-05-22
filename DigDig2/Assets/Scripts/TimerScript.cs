using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Vector3 start, end, current;
    [SerializeField] GameObject back, MainCamera;
    [SerializeField] float timedPos;
    [SerializeField] float timer;
    [SerializeField] float timeMulti = 0.1f;
    [SerializeField] float dayTime = 300;
    [SerializeField] Vector3 flipslide, flippening, done, positionAdjustment;
    [SerializeField] Sprite[] dayNightSprites;
    [SerializeField] bool night;
    [SerializeField] bool fliped;
    bool flipping;
    [SerializeField] float flipspeed;
    [SerializeField] Vector4 currentColor, nightColor, dayColor;
    [SerializeField] SpriteRenderer nightCover;
    [SerializeField] GameStateManager gsManager;
    [SerializeField] RoundManager roundManager;


    void Start()
    {
        currentColor = dayColor;
        current = back.transform.position + start;
        current.z = 90;
        timeMulti = end.x / dayTime;
        flipspeed = flippening.y / 0.5f;
        gsManager.OnGameStateChange += OnGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!flipping) 
        {
            timedPos += Time.deltaTime * timeMulti;
            timer = timedPos;
        }
        if (night && timer >= 1)
        {
            roundManager.EndDay();
        }
        current.x = start.x + (timedPos * 2) + back.transform.position.x;
        current.y = back.transform.position.y;
        transform.position = current;

        if(timedPos >= 1)
        {
            night = !night;
            flipping = true;
            timer = 0;
            roundManager.time = 300;
        }
        if (flipping)
        {
            timedPos -= Time.deltaTime * end.x * (flipspeed / 2);
            if (!night)
            {
                timeMulti = end.x / dayTime;
            }
        }
        
        if (timedPos < 0)
        {
            timedPos = 0;
            flipping = false;
        }
        ColorChange();
        done = MainCamera.transform.position + positionAdjustment;
        if (night)
        {
             
            if (!fliped)
            {
                flipslide.y += Time.deltaTime * flipspeed;
            }
            if (MainCamera.transform.position.y + flipslide.y > done.y + flippening.y)
            {
                flipslide.y = done.y - MainCamera.transform.position.y + flippening.y;
                back.GetComponent<SpriteRenderer>().sprite = dayNightSprites[1];
                fliped = true;
            }
            if (fliped)
            {
                flipslide.y -= Time.deltaTime * flipspeed;
            }
            if (flipslide.y < done.y - MainCamera.transform.position.y)
            {
                flipslide.y = done.y - MainCamera.transform.position.y;
            }
        }
        else
        {
            roundManager.time = timer * 300;

            if (fliped)
            {
                flipslide.y += Time.deltaTime * flipspeed;
            }
            if (MainCamera.transform.position.y + flipslide.y > done.y + flippening.y)
            {
                flipslide.y = done.y - MainCamera.transform.position.y + flippening.y;
                back.GetComponent<SpriteRenderer>().sprite = dayNightSprites[0];
                fliped = false;
            }
            if (!fliped)
            {
                flipslide.y -= Time.deltaTime * flipspeed;
            }
            if (flipslide.y < done.y - MainCamera.transform.position.y)
            {
                flipslide.y = done.y - MainCamera.transform.position.y;
            }
        }
        back.transform.position = MainCamera.transform.position + flipslide;
        nightCover.color = currentColor;
        
    }
    public void SetDay()
    {
        timeMulti = 1;
    }
    void ColorChange()
    {
        if (!night && timer > 0.7f)
        {
            currentColor.w = nightColor.w * Mathf.Pow((timer - 0.7f) / 0.3f, 3);
            if (currentColor.w > nightColor.w)
            {
                currentColor.w = nightColor.w;
            }
            currentColor.y = dayColor.y - ((timer - 0.7f) / 0.15f);
            if (currentColor.y < nightColor.y)
            {
                currentColor.y = nightColor.y;
            }
            if (timer > 0.85f)
            {
                currentColor.x = dayColor.x - (timer - 0.85f) / 0.15f;
                if (currentColor.x < nightColor.x)
                {
                    currentColor.x = nightColor.x;
                }
                currentColor.z = nightColor.z * ((timer - 0.85f) / 0.15f);
                if (currentColor.z > nightColor.z)
                {
                    currentColor.z = nightColor.z;
                }
            }
        }
        if (!night && timer < 0.7f)
        {
            currentColor = dayColor;
        }
        if (night && timer > 0.7f)
        {
            currentColor.w = nightColor.w - (dayColor.w + nightColor.w) * Mathf.Pow((timer - 0.7f) / 0.3f, 1f/3);
            if (currentColor.w < dayColor.w)
            {
                currentColor.w = dayColor.w;
            }
            currentColor.x = nightColor.x + (dayColor.x - nightColor.x) * ((timer - 0.7f) / 0.15f);
            if (currentColor.x > dayColor.x)
            {
                currentColor.x = dayColor.x;
            }
            currentColor.z = nightColor.z - (dayColor.z + nightColor.z) * ((timer - 0.7f) / 0.15f);
            if (currentColor.z < dayColor.z)
            {
                currentColor.z = dayColor.z;
            }
            if (timer > 0.85)
            {
                
                currentColor.y = (timer - 0.85f) / 0.15f;
                if (currentColor.y > dayColor.y)
                {
                    currentColor.y = dayColor.y;
                }
            }
        }
        if (night && timer < 0.7f)
        {
            currentColor = nightColor;
        }
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }
}
