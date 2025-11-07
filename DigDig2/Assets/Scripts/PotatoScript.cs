using UnityEngine;

public class PotatoScript : MonoBehaviour
{
    PlantDeath Life;
    public int damageValue = 1;
    private void Awake()
    {
        Life = GetComponent<PlantDeath>();
    }
    
}
