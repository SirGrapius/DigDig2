using UnityEngine;

public class HealerScript : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float time;
    [SerializeField] float frequency;
    [SerializeField] int heal;
    void Update()
    {
        time += Time.deltaTime;
        if (time >= frequency)
        {
            GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
            plants = RangeLimit(plants);
            for (int i = 0; i < plants.Length; i++)
            {
                PlantDeath life = plants[i].GetComponent<PlantDeath>();
                if (life.hpMax > life.hp)
                {
                    life.hp += heal;
                }
            }
            time -= frequency;
        }
    }
    GameObject[] RangeLimit(GameObject[] plants)
    {
        GameObject[] plantsInRange = plants;
        int counter = 0;
        for (int i = 0; i < plants.Length; i++)
        {
            Vector3 centered = plants[i].transform.position - transform.position;
            if (centered.sqrMagnitude < range * range)
            {
                plantsInRange[counter] = plants[i];
                counter++;
            }
        }
        plants = plantsInRange[..counter];
        return plants;
    }
}
