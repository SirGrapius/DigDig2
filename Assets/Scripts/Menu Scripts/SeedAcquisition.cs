using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeedAcquisition: MonoBehaviour
{
    [SerializeField] Tilemap inventory;
    [SerializeField] Vector3Int tilepos;
    [SerializeField] Tile seedType;
    [SerializeField] GameStateManager wallet;
    [SerializeField] int cost;
    void OnMouseDown()
    {
        if (wallet.heldMoneyAmount >= cost)
        {
            wallet.heldMoneyAmount -= cost;
            wallet.moneyUI.GetComponent<TextMeshPro>().text = wallet.heldMoneyAmount.ToString();
            inventory.SetTile(tilepos, seedType);
            Destroy(gameObject);
        }
    }
}
