using UnityEngine;
using UnityEngine.Tilemaps;

public class ShopingScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D myCollider;
    [SerializeField] GameObject shopOverlay;
    [SerializeField] Tile[] possiblePlantsSold;
    [SerializeField] Tile[] plantsCurrentlySold;
    [SerializeField] int plantsSold;
    [SerializeField] int[] possibleItemsSold;
    [SerializeField] int[] itemsCurrentlySold;
    [SerializeField] int itemsSold;
    [SerializeField] bool shopOpen;
    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        shopOverlay = Camera.main.transform.GetChild(2).gameObject;
        plantsCurrentlySold = new Tile[plantsSold];
        for (int i = 0; i < plantsSold; i++)
        {
            plantsCurrentlySold[i] = possiblePlantsSold[i];
        }
    }
    void Update()
    {
        if (shopOpen)
        {

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        shopOverlay.SetActive(true);
        shopOpen = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        shopOverlay.SetActive(false);
        shopOpen = false;
    }
}
