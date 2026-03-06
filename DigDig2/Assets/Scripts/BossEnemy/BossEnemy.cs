using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] float relocateCooldown = 30;
    [SerializeField] float relocateTimer = 30;
    [SerializeField] float baseAttackCooldown = 12;
    [SerializeField] float attackTimer = 12;
    [SerializeField] float stunRadius = 9;
    [SerializeField] float stunDuration = 3f;
    private bool lastAttackWasCrow = false;
    private int lastPoint = -1;
    private int newPoint;
    

    [SerializeField] bool isRelocating = false;

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

            animator.SetTrigger("Attack");
            
            if (lastAttackWasCrow == true)
            {
                if (roll <= 7) StunAttack();
                else if (roll <= 9) DeAgeAttack();
                else CrowAttack();
            }
            else if (lastAttackWasCrow == false)
            {
                if (roll <= 7) StunAttack();
                else DeAgeAttack();
            }
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
        int newPoint = Random.Range(0, points.Length - 1);

        if (newPoint >= lastPoint) newPoint++;

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

    public void DescendFinished()
    {
        isRelocating = false;
    }

    private void StunAttack()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        if (plants.Length == 0)
        {
            if (lastAttackWasCrow == true)
            {
                DeAgeAttack();
            }
            else if (lastAttackWasCrow == false)
            {
                int roll = Random.Range(1, 11);

                if (roll <= 7) DeAgeAttack();
                else DeAgeAttack();
            }
            return;
        }

        Transform center = plants[Random.Range(0, plants.Length)].transform;

        foreach (GameObject p in plants)
        {
            if (Vector3.Distance(center.position, p.transform.position) <= stunRadius)
            {
                if (p.GetComponentInChildren<CottonScript>() != null)
                {
                    CottonScript script = p.GetComponentInChildren<CottonScript>(); script.Stun(stunDuration); }
                else if (p.GetComponentInChildren<Chiliscript>() != null)
                { Chiliscript script = p.GetComponentInChildren<Chiliscript>(); script.Stun(stunDuration); }
                else if (p.GetComponentInChildren<PotatoScript>() != null)
                { PotatoScript script = p.GetComponentInChildren<PotatoScript>(); script.Stun(stunDuration); }

            }
        }

        attackTimer = baseAttackCooldown * 1f;
        lastAttackWasCrow = false;
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
            if (p.GetComponentInChildren<CottonScript>() != null)
            {
                CottonScript script = p.GetComponentInChildren<CottonScript>();

                if (script != null)
                {
                    script.BecomeBaby();
                    plants.Remove(p);
                }
            }
            else if (p.GetComponentInChildren<Chiliscript>() != null)
            {
                Chiliscript script = p.GetComponentInChildren<Chiliscript>();

                if (script != null)
                {
                    script.BecomeBaby();
                    plants.Remove(p);
                }
            }
            else if (p.GetComponentInChildren<PotatoScript>() != null)
            {
                PotatoScript script = p.GetComponentInChildren<PotatoScript>();

                if (script != null)
                {
                    script.BecomeBaby();
                    plants.Remove(p);
                }
            }
        }

        attackTimer = baseAttackCooldown * 1.4f;
        lastAttackWasCrow = false;
    }

    private void CrowAttack()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("GrowingPlant");

        foreach (GameObject p in plants)
        {
            GameObject crow = Instantiate(crowPrefab, transform.position, Quaternion.identity);
            CrowProjectile crowScript = crow.GetComponent<CrowProjectile>();

            crowScript.targetplant = p;
        }

        attackTimer = baseAttackCooldown * 2f;
        lastAttackWasCrow = true;
    }
}  







