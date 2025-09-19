using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //notes for enemy spawning
    //R × ((10×2^(R ÷ 2))÷100)+1 rounded out = enemy points
    [Header("Day Settings")]
    [SerializeField] int day;
    [SerializeField] float time;
    [SerializeField] float maxRoundTime = 300;

    [Header("Lane Settings")]
    [SerializeField] GameObject[] lanes;
    [SerializeField] bool[] isLaneOpen;

    [Header("Enemy Settings")]
    [SerializeField] int numberOfEnemies;
    [SerializeField] int enemyPoints;
    [SerializeField] GameObject[] enemyPrefabs;

    void Start()
    {
        day = 1;
        bool rightLaneOpen = isLaneOpen[0];
        bool leftLaneOpen = isLaneOpen[1];
        bool upLaneOpen = isLaneOpen[2];
    }


    void Update()
    {
        if (time < maxRoundTime) //change time to Time.deltaTime
        {
            time += Time.deltaTime;
        }
        if (time >= maxRoundTime && numberOfEnemies == 0) //if 5 minutes have passed and all enemies are dead progress to the next day
        {
            EndDay();
        }
        if (time == 60 || time == 120 || time == 180 || time == 240) //generate enemy points every minute
        {
            enemyPoints = Mathf.RoundToInt(day * ((day * Mathf.Pow(2, day / 2)) / 100)+1);
        }

        if (enemyPoints >= 0)
        {
            SpawnEnemies();
        }
    }

    void OpenNewLane()
    {
        if (day == 3)
        {
            isLaneOpen[0] = true;
        }

        if (day == 5)
        {
            isLaneOpen[1] = true;
        }

        if (day == 7)
        {
            isLaneOpen[2] = true;
        }
    }

    void SpawnEnemies()
    {
        int whatEnemy;
        int whatLane;
        float maxLane = 0;

        Vector3[] v = new Vector3[3];

        if (!isLaneOpen[0])
        {
            maxLane = 0;
        }
        else if (!isLaneOpen[1])
        {
            maxLane = 1;
        }
        else if (!isLaneOpen[2])
        {
            maxLane = 2;
        }
        else
        {
            maxLane = 3;
        }

            for (int i = 0; i < enemyPoints; i++)
            {
                whatLane = Mathf.RoundToInt(Random.Range(0, maxLane));
                whatEnemy = Mathf.RoundToInt(Random.Range(0, 2));
                switch (whatLane)
                {
                    case 0: //down
                    {
                        RectTransform laneRect = lanes[whatLane].GetComponent<RectTransform>();
                        laneRect.GetWorldCorners(v);
                        //left
                        //v[0] =
                        //left
                        //v[1] =
                        //left
                        //v[2] =
                        //left
                        //v[3] =

                        break;
                    }


                }

                if (day < 3)
                {
                    Instantiate(enemyPrefabs[0], lanes[whatLane].transform);
                }
                else
                {
                    Instantiate(enemyPrefabs[whatEnemy], lanes[whatLane].transform);
                }
            }
    }



    void EndDay()
    {
        day++;
        time = 0;
    }
}
