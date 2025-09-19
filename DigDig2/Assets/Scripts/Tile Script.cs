using UnityEngine;
using UnityEngine.Tilemaps;

public class TileScript : MonoBehaviour
{
    [SerializeField] Tilemap myTilemap;
    [SerializeField] TilemapCollider2D myCollider;
    [SerializeField] Tile myTile;
    [SerializeField] Camera myCamera;
    [SerializeField] Vector3Int myTilePositionInt;
    [SerializeField] Vector3 myTilePosition;
    [SerializeField] Transform tileSelectBorder;
    [SerializeField] float selectionTimer;

    [SerializeField] bool isInventory;
    [SerializeField] Tile selectedInventoryTile;
    [SerializeField] int selectedTool;
    // 1 = hoe
    // 2 = watering can
    // 3 = shovel
    [SerializeField] Tile tilledSoil;
    [SerializeField] Tile unTilledSoil;
    [SerializeField] GameObject myInventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // setting variables to components
        myTilemap = GetComponent<Tilemap>();
        myCollider = GetComponent<TilemapCollider2D>();
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // making it so that the border disapears if the timer ends
        selectionTimer -= Time.deltaTime;
        if (selectionTimer <= 0 && !isInventory)
        {
            tileSelectBorder.position += new Vector3(1000, 1000, 0);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedTool > 1)
            {
                selectedTool -= 1;
            }
            else
            {
                selectedTool = 3;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedTool < 3)
            {
                selectedTool += 1;
            }
            else
            {
                selectedTool = 1;
            }
        }

    }

    private void OnMouseOver()
    {
        // selecting a tile to change
        if (!isInventory)
        {
            myTilePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
            myTilePosition /= transform.localScale.x;
            myTilePositionInt = Vector3Int.FloorToInt(myTilePosition);
            myTilePositionInt.z = 0;
            tileSelectBorder.position = myTilePositionInt;
            selectionTimer = 0.1f;
            myTile = myTilemap.GetTile<Tile>(myTilePositionInt);
        }
    }
    private void OnMouseDown()
    {
        // selecting a tile from the isInventory
        if (isInventory)
        {
            myTilePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
            myTilePosition /= transform.localScale.x;
            myTilePositionInt = Vector3Int.FloorToInt(myTilePosition);
            myTilePositionInt.z = 0;
            selectedInventoryTile = myTilemap.GetTile<Tile>(myTilePositionInt);
        }
        // using a tool or seed on a tile
        if (!isInventory)
        {
            if (myTile == tilledSoil)
            {
                myTilePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
                myTilePosition /= transform.localScale.x;
                myTilePositionInt = Vector3Int.FloorToInt(myTilePosition);
                myTilePositionInt.z = 0;
                selectedInventoryTile = myInventory.GetComponent<TileScript>().selectedInventoryTile;
                myTilemap.SetTile(myTilePositionInt, selectedInventoryTile);
            }
            else if (selectedTool == 1 && myTile == unTilledSoil)
            {
                myTilePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
                myTilePosition /= transform.localScale.x;
                myTilePositionInt = Vector3Int.FloorToInt(myTilePosition);
                myTilePositionInt.z = 0;
                selectedInventoryTile = myInventory.GetComponent<TileScript>().selectedInventoryTile;
                myTilemap.SetTile(myTilePositionInt, tilledSoil);
            } 
        }
    }
}
