using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GrowupNowFuckers : MonoBehaviour
{

    void Update()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        foreach (GameObject p in plants)
        {
            CottonScript script = p.GetComponent<CottonScript>();

            if (script != null)
            {
                script.growthTimer = 89;
            }
        }
    }
}
