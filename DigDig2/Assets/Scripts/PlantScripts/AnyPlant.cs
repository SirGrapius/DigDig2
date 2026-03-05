using UnityEngine;

public class AnyPlant : MonoBehaviour
{
    [SerializeField] CottonScript typeCotton;
    [SerializeField] PotatoScript typePotato;
    [SerializeField] Chiliscript typeChili;

    public bool IsGrowing()
    {
        if (typeCotton != null && typeCotton.growing)
        {
            return true;
        }
        if (typePotato != null && typePotato.growing)
        {
            return true;
        }
        if (typeChili != null && typeChili.growing)
        {
            return true;
        }
        return false;
    }
    public bool IsInInventory()
    {
        if (typeCotton != null && typeCotton.isInInventory)
        {
            return true;
        }
        if (typePotato != null && typePotato.isInInventory)
        {
            return true;
        }
        if (typeChili != null && typeChili.isInInventory)
        {
            return true;
        }
        return false;
    }
}
