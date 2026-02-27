using UnityEngine;

public class AnyPlant : MonoBehaviour
{
    CottonScript typeCotton;
    PotatoScript typePotato;
    Chiliscript typeChili;
    public bool IsGrowing()
    {
        typeCotton = gameObject.GetComponent<CottonScript>();
        typePotato = gameObject.GetComponent<PotatoScript>();
        typeChili = gameObject.GetComponent<Chiliscript>();
        if (typeCotton != null && typeCotton.growing)
        {
            return true;
        }
        if (typePotato != null && typePotato.growing)
        {
            return true;
        }
        if (typeChili != null && typeChili)
        {
            return true;
        }
        return false;
    }
}
