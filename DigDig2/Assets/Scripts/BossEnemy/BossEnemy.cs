using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] float relocateCooldown = 30;
    [SerializeField] float relocateTimer = 30;
    [SerializeField] float attackCooldown = 12;
    [SerializeField] float attackTimer = 12;
    [SerializeField] float stunRadius = 9;
    [SerializeField] float stunDuration = 3f;
    private int lastPoint = -1;
    private int newPoint;

    private bool isRelocating = false;
    
    [SerializeField] GameObject crowPrefab;

    [SerializeField] Animator animator;

    [SerializeField] Transform[] points;

    private void Update()
    {
        if (relocateTimer <= 0)
        {
            Ascend();
            relocateTimer = relocateCooldown;
        }
        else
        {
            relocateTimer -= Time.deltaTime;
        }

        if (attackTimer <= 0 && isRelocating != true)
        {
            int roll = Random.Range(1, 11);

            if (roll <= 7) StunAttack();
            else if (roll <= 9) DeAgeAttack();
            else CrowAttack();
        }
        else if (isRelocating != true)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void Ascend() 
    {
        isRelocating = true;
        animator.SetTrigger("Ascend");
    }
    
    private void Descend()
    {
        do
        {
            newPoint = Random.Range(0, points.Length);
        }
        while (newPoint == lastPoint); // Remake and exlude lastPoint from random.range instead of looping

        switch (newPoint)
        {
            case 0:
                    animator.SetInteger("Direction", 0);
                    break;

            case 1:
                    animator.SetInteger("Direction", 1);
                    break;

            case 2:
                    animator.SetInteger("Direction", 2);
                    break;

            case 3:
                    animator.SetInteger("Direction", 3);
                    break;

        }

        transform.position = points[newPoint].transform.position; 

        lastPoint = newPoint;

        animator.SetTrigger("Descend");
    }

    private void DescendFinished()
    {
        isRelocating = false;
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
                if (p.GetComponentInChildren<CottonScript>() != null)
                { CottonScript script = p.GetComponentInChildren<CottonScript>(); script.Stun(stunDuration); }
                else if (p.GetComponentInChildren<PotatoScript>() != null)
                { PotatoScript script = p.GetComponentInChildren<PotatoScript>(); script.Stun(stunDuration); }
                else if (p.GetComponentInChildren<Chiliscript>() != null)
                { Chiliscript script = p.GetComponentInChildren<Chiliscript>(); script.Stun(stunDuration); }
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
            GameObject crow = Instantiate(crowPrefab, transform.position, Quaternion.identity);
            CrowProjectile crowScript = crow.GetComponent<CrowProjectile>();

            crowScript.targetplant = p.transform.position;
        }

        attackTimer = attackCooldown * 2f; 
    }





   // Add finding all plant scripts, add boss and raven projectile animations/graphic, apply stun effect on plants hit and stun radius effect, add beeing able to hit crows 
   // Add "ascent" function where it plays the ascend animation, and at the end of animation add keyframe that calls "descent" function, where it selects a random zone or point aside from current zone, teleports, and then plays descend animation. 
}  // check if already crow flying? make attacks not fully random?







