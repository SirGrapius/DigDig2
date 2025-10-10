using UnityEngine;
using UnityEngine.Tilemaps;

public class TileScript : MonoBehaviour
{
    public Tilemap myTilemap;
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
    public Tile tilledSoil;
    [SerializeField] Tile unTilledSoil;
    [SerializeField] GameObject myInventory;
    [SerializeField] float useTimer;
    [SerializeField] float useTimerMax;

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
        if (Input.GetMouseButtonUp(0))
        {
            useTimer = 0;
        }
    }

    private void OnMouseOver()
    {
        // selecting a tile to change
        if (!isInventory)
        {
            myTilePositionInt = CheckTile();
            tileSelectBorder.position = myTilePositionInt * (int)transform.parent.GetComponent<Grid>().cellSize.x;
            selectionTimer = 0.1f;
            if (myTile != myTilemap.GetTile<Tile>(myTilePositionInt))
            {
                useTimer = 0;
                myTile = myTilemap.GetTile<Tile>(myTilePositionInt);
            }
        }
    }
    private void OnMouseDrag()
    {
        // selecting a tile from the isInventory
        if (isInventory)
        {
            myTilePositionInt = CheckTile();
            selectedInventoryTile = myTilemap.GetTile<Tile>(myTilePositionInt);
        }
        // using a tool or seed on a tile
        if (!isInventory)
        {
            if (myTile == tilledSoil)
            {
                if (useTimer == 0)
                {
                    myTilePositionInt = CheckTile();
                    useTimer += Time.deltaTime;                
                }
                else if (useTimer < useTimerMax && useTimer > 0)
                {
                    useTimer += Time.deltaTime;
                }
                if (useTimer > useTimerMax)
                {
                    selectedInventoryTile = myInventory.GetComponent<TileScript>().selectedInventoryTile;
                    myTilemap.SetTile(myTilePositionInt, selectedInventoryTile);
                    useTimer = 0;
                }
            }
            else if (selectedTool == 1 && myTile == unTilledSoil)
            {

                if (useTimer == 0)
                {
                    myTilePositionInt = CheckTile();
                    useTimer += Time.deltaTime;
                }
                else if (useTimer < useTimerMax && useTimer > 0)
                {
                    useTimer += Time.deltaTime;
                }
                if (useTimer > useTimerMax)
                {
                    selectedInventoryTile = myInventory.GetComponent<TileScript>().selectedInventoryTile;
                    myTilemap.SetTile(myTilePositionInt, tilledSoil);
                    useTimer = 0;
                }
            }
        }
    }
    public Vector3Int CheckTile()
    {
        Vector3 tilePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        tilePosition /= transform.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x;
        Vector3Int tilePositionInt = Vector3Int.FloorToInt(tilePosition);
        tilePositionInt.z = 0;
        return tilePositionInt;
    }
}
