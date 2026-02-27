using UnityEngine;

public class AnyPlant : MonoBehaviour
{
    CottonScript typeCotton;
    PotatoScript typePotato;
    Chiliscript typeChili;
    void Start()
    {
        typeCotton = gameObject.GetComponent<CottonScript>();
        typePotato = gameObject.GetComponent<PotatoScript>();
        typeChili = gameObject.GetComponent<Chiliscript>();
    }

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
}
