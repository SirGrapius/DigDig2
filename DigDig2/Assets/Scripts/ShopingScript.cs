using UnityEngine;
using UnityEngine.Tilemaps;

public class ShopingScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D myCollider;
    [SerializeField] Tile[] possiblePlantsSold;
    [SerializeField] Tile[] plantsCurrentlySold;
    [SerializeField] int plantsSold;
    [SerializeField] int[] possibleItemsSold;
    [SerializeField] int[] itemsCurrentlySold;
    [SerializeField] int itemsSold;
    [SerializeField] bool shopOpen;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        shopOpen = !shopOpen;
    }
}
