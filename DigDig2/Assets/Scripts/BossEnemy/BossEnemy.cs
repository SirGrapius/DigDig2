using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] float relocateCooldown = 30;
    [SerializeField] float relocateTimer = 30;
    [SerializeField] float attackCooldown = 12;
    [SerializeField] float attackTimer = 12;
    [SerializeField] float stunRadius = 9;
    [SerializeField] float stunDuration = 3f;

    private bool isTeleporting = false;

    [SerializeField] GameObject crowPrefab;

    [SerializeField] List<Collider2D> zones;
    Collider2D currentZone;

    private void Update()
    {
        if (relocateTimer <= 0)
        {
            Relocate();
            relocateTimer = relocateCooldown;
        }
        else
        {
            relocateTimer -= Time.deltaTime;
        }

        if (attackTimer <= 0)
        {
            int roll = Random.Range(1, 11);

            if (roll <= 7) StunAttack();
            else if (roll <= 10) DeAgeAttack(); 
            else CrowAttack();
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void Relocate() // Make a temp list with zones, remove current zone from it, and then do a random.range between 0 and list.count. 
    {
        // Use choosepointinzone to get a random point for teleportatiion, set isteleporting to ture and start a quarantine or delay for animation.length before setting it to false
    }

    Vector3 ChoosePointInZone()
    {
        return new Vector3(1, 1, 1); // Remove this, add Get random point in selected zone 
    }

    private void StunAttack()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        if (plants.Length == 0) return;

        Transform center = plants[Random.Range(0, plants.Length)].transform;

        foreach (GameObject p in plants)
        {
            if (Vector3.Distance(center.position, p.transform.position) <= stunRadius)
            {
                CottonScript script = p.GetComponentInChildren<CottonScript>(); 
                if (script != null) 
                script.Stun(stunDuration);
            }
        }

        attackTimer = attackCooldown * 1f;
    }

    private void DeAgeAttack() 
    {
        List<GameObject> plants = new List<GameObject>(GameObject.FindGameObjectsWithTag("Plant"));
        if (plants.Count == 0) return;

        int count = Mathf.Min(5, plants.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, plants.Count); 
            GameObject p = plants[randomIndex];
            CottonScript script = p.GetComponentInChildren<CottonScript>(); 
            if (script != null)
            script.BecomeBaby();
            plants.Remove(p);
        }

        attackTimer = attackCooldown * 1.4f;
    }

    private void CrowAttack()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("GrowingPlant");

        foreach (GameObject p in plants)
        {
            CottonScript script = p.GetComponentInChildren<CottonScript>();
            if (script != null)
            {
                GameObject crow = Instantiate(crowPrefab, transform.position, Quaternion.identity); 
                CrowProjectile crowScript = crow.GetComponent<CrowProjectile>();
               
                crowScript.targetplant = p.transform.position;
            }
        }

        attackTimer = attackCooldown * 2f; 
    }





   // Add relocation, add finding all plant scripts, add boss and raven projectile animations/graphic, apply stun effect on plants hit
}







