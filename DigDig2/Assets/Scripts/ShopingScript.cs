using UnityEngine;
using UnityEngine.Tilemaps;

public class ShopingScript : MonoBehaviour
{
    [SerializeField] Tile[] possiblePlantsSold;
    [SerializeField] Tile[] plantsCurrentlySold;
    [SerializeField] int plantsSold;
    [SerializeField] int[] possibleItemsSold;
    [SerializeField] int[] itemsCurrentlySold;
    [SerializeField] int itemsSold;
    private void Awake()
    {
        plantsCurrentlySold = new Tile[plantsSold];
        for (int i = 0; i < plantsSold; i++)
        {
            plantsCurrentlySold[i] = possiblePlantsSold[i];
        }
    }
    void Update()
    {
        
    }
}
