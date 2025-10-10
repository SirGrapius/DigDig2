using System.Collections;
using TMPro;
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
    [SerializeField] GameObject bossPrefab;
    bool generatingPoints;
    bool spawningEnemy;

    [Header("Text Settings")]
    [SerializeField] TextMeshProUGUI myText;
    [SerializeField] float fadeDuration;
    [SerializeField] float textDuration;

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
        if (( (60 <= time && time < 61) || (120 <= time && time < 121) || (180 <= time && time < 181) || (240 <= time && time < 241 && day != 10)) && !generatingPoints) //generate enemy points every minute
        {
            StartCoroutine(TextFadeCoroutine(new Color(myText.color.r,myText.color.g,myText.color.b,0),new Color(myText.color.r,myText.color.g,myText.color.b,1), "A Wave of Beasts is Coming, Prepare Yourself!"));
            StartCoroutine(GenerateEnemyPoints());
        }

        if ((240 <= time && time < 241) && day >= 10) //creates special text and spawns a boss on the final wave on all days including and follow the tenth
        {
            StartCoroutine(TextFadeCoroutine(new Color(myText.color.r, myText.color.g, myText.color.b, 0), new Color(myText.color.r, myText.color.g, myText.color.b, 1), "A Terrifying Foe Approaches From Below! A Massive Wave of Beasts is Coming!"));
            StartCoroutine(GenerateEnemyPoints());
            Instantiate(bossPrefab, lanes[0].transform.position, Quaternion.identity);
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
        if (day == 3) //opens the right lane on the third day
        {
            isLaneOpen[0] = true;
            StartCoroutine(TextFadeCoroutine(new Color(myText.color.r, myText.color.g, myText.color.b, 0), new Color(myText.color.r, myText.color.g, myText.color.b, 1), "The Beasts Have Opened a New Path, They Can Come From The Right Side Now..."));
        }
        
        if (day == 5) //opens the top lane on the fifth day
        {
            isLaneOpen[1] = true;
            StartCoroutine(TextFadeCoroutine(new Color(myText.color.r, myText.color.g, myText.color.b, 0), new Color(myText.color.r, myText.color.g, myText.color.b, 1), "The Beasts Have Opened a New Path, They Can Come From The Top Side Now..."));
        }

        if (day == 7) //opens the left lane on the seventh day
        {
            isLaneOpen[2] = true;
            StartCoroutine(TextFadeCoroutine(new Color(myText.color.r, myText.color.g, myText.color.b, 0), new Color(myText.color.r, myText.color.g, myText.color.b, 1), "The Beasts Have Opened a New Path, They Can Come From The Left Side Now..."));
        }

        if (day == 10) //special text warning of the boss on the tenth day
        {
            StartCoroutine(TextFadeCoroutine(new Color(myText.color.r, myText.color.g, myText.color.b, 0), new Color(myText.color.r, myText.color.g, myText.color.b, 1), "The Ground Rumbles, Something Terrifying is Coming Today!"));
        }
    }

    IEnumerator TextFadeCoroutine(Color startColor, Color targetColor, string textText)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;
        myText.text = textText;
        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / fadeDuration;
            myText.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        yield return new WaitForSeconds(fadeDuration);
        yield return new WaitForSeconds(textDuration);
        StartCoroutine(TextFadeOutCoroutine(targetColor, startColor));
    }
    IEnumerator TextFadeOutCoroutine(Color startColor, Color targetColor)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;
        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / fadeDuration;
            myText.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        yield return new WaitForSeconds(fadeDuration);
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
            whatEnemy = Mathf.RoundToInt(Random.Range(0, 2)); //decides what enemy to spawn
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
        OpenNewLane();
        time = 0;
    }
}
