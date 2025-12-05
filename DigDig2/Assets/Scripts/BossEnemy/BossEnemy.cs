using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] float relocateCooldown = 30;
    [SerializeField] float relocateTimer = 30;
    [SerializeField] float attackCooldown = 12;
    [SerializeField] float attackTimer = 12;
    [SerializeField] float stunRadius = 3;

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

            if (roll <= 5) StunAttack();
            else if (roll <= 9) DeAgeAttack(); 
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
                CottonScript script = p.GetComponent<CottonScript>(); // Add Stun function inside plant script
                // if (script != null)
                //    script.Stun();
            }
        }

        attackTimer = attackCooldown * 1f;
    }

    private void DeAgeAttack()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        if (plants.Length == 0) return;

        int count = Mathf.Min(5, plants.Length);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, plants.Length);
            GameObject p = plants[randomIndex];
            CottonScript script = p.GetComponentInChildren<CottonScript>();
            if (script != null)
                script.BecomeBaby();
        }

        attackTimer = attackCooldown * 1.2f;
    }

    private void CrowAttack()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        foreach (GameObject p in plants)
        {
            CottonScript script = p.GetComponent<CottonScript>();
            if (script != null && script.baseAnimator.GetBool("Child"))
            {
                GameObject crow = Instantiate(crowPrefab, transform.position, Quaternion.identity); // Make instantiated crow movetowards its selected plant, maybe in its own script, and when it arrives start eating animation
            }
        }

        attackTimer = attackCooldown * 2f;
    }




    // De aging attack, 10 plants close together, stun attack, eating attack find all plants with time less than "" and instantiate a raven which uses movetowards. When hit by player dissapears, when arriving, if plant is stage 2/over a time, dissapear. If plant is young spends 1 second animatin, at the end if not attacked by player, eat plant and dissapear.
    // When attackCooldown reaches 0 and if "relocating" = false, make a random.range between 1 and 10, 1-4 equals common attack, 5-8 uncommon attack and 9-10.
    // After "relocate time" has passed, set relocate time to 0, and choose a random position within the lanes list except for current lane. 
}







