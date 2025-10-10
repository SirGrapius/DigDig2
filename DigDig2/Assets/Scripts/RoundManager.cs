using System.Collections;
using UnityEditor.ShaderGraph.Internal;
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
    [SerializeField] GameObject[] lanes; //0 = bottom, 1 = right, 2 = top, 3 = left
    [SerializeField] bool[] isLaneOpen;

    [Header("Enemy Settings")]
    [SerializeField] int numberOfEnemies;
    [SerializeField] int enemyPoints;
    [SerializeField] GameObject[] enemyPrefabs;
    bool generatingPoints;
    bool spawningEnemy;

    void Start()
    {
        day = 1;
        bool rightLaneOpen = isLaneOpen[0];
        bool leftLaneOpen = isLaneOpen[1];
        bool upLaneOpen = isLaneOpen[2];
        generatingPoints = false;
    }


    void Update()
    {
        if (time < maxRoundTime) //changes time to Time.deltaTime
        {
            time += Time.deltaTime;
        }
        if (time >= maxRoundTime && numberOfEnemies == 0) //if 5 minutes have passed and all enemies are dead progress to the next day
        {
            EndDay();
        }
        if (((0 <= time && time < 1) || (60 <= time && time < 61) || (120 <= time && time < 121) || (180 <= time && time < 181) || (240 <= time && time < 241)) && !generatingPoints) //generate enemy points every minute
        {
            StartCoroutine(GenerateEnemyPoints());
        }

        if (enemyPoints > 0 && !spawningEnemy)
        {
            spawningEnemy = true;
            StartCoroutine(SpawnEnemies());
        }

        numberOfEnemies = Mathf.RoundToInt(GameObject.FindGameObjectsWithTag("Enemy").Length);
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

    IEnumerator GenerateEnemyPoints()
    {
        generatingPoints = true;
        enemyPoints = Mathf.RoundToInt(day * ((day * Mathf.Pow(2, day / 2)) / 100) + 1);
        yield return new WaitForSeconds(2);
        generatingPoints = false;
        yield return null;
    }

    IEnumerator SpawnEnemies()
    {
        int whatEnemy;
        int whatLane;
        float maxLane = 0;

        Vector3[] v = new Vector3[4];

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
        else if (isLaneOpen[2])
        {
            maxLane = 3;
        }
        for (int i = 0; i < enemyPoints; i++) //spawns an amount of enemy equal to the amount of enemy points
        {
            yield return new WaitForSeconds(Random.Range(1, 4));
            whatLane = Mathf.RoundToInt(Random.Range(0, maxLane)); //decides what lane the enemy will spawn on
            whatEnemy = Mathf.RoundToInt(Random.Range(0, 2));
            BoxCollider2D laneCollider = lanes[whatLane].GetComponent<BoxCollider2D>();
            float topSide = laneCollider.size.y + lanes[whatLane].transform.position.y;
            float bottomSide = -laneCollider.size.y + lanes[whatLane].transform.position.y;
            float rightSide = laneCollider.size.x + lanes[whatLane].transform.position.x;
            float leftSide = -laneCollider.size.x + lanes[whatLane].transform.position.x;
            Vector3 spawnPos = new Vector3(Random.Range(leftSide, rightSide), Random.Range(bottomSide, topSide), 0);
            if (day < 3)
            {
                Instantiate(enemyPrefabs[0], spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(enemyPrefabs[whatEnemy], spawnPos, Quaternion.identity);
            }
        }
        enemyPoints = 0;
        spawningEnemy = false;
        yield return null;
    }



    void EndDay()
    {
        day++;
        time = 0;
    }
}
