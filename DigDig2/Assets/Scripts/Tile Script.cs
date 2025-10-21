using UnityEngine;
using UnityEngine.Tilemaps;

public class TileScript : MonoBehaviour
{
    [Header("Variables for selecting tiles")]
    public Tilemap myTilemap;
    [SerializeField] TilemapCollider2D myCollider;
    [SerializeField] Tile myTile;
    [SerializeField] Camera myCamera;
    [SerializeField] Vector3Int myTilePositionInt;
    [SerializeField] Vector3 myTilePosition;
    [SerializeField] Transform tileSelectBorder;
    [SerializeField] float selectionTimer;

    [Header("Planting and Inventory")]
    public bool isInventory;
    [SerializeField] Tile selectedInventoryTile;
    public int selectedTool;
    // 1 = hoe
    // 2 = watering can
    // 3 = shovel
    public Tile tilledSoil;
    public Tile unTilledSoil;
    [SerializeField] GameObject myInventory;

    [Header("Variables for use of Tools")]
    [SerializeField] float useTimer;
    [SerializeField] float useTimerMax;

    [SerializeField] GameObject pickedUpPlant;
    [SerializeField] Tile pickedUpPlantType;
    [SerializeField] Vector3 pickedUpPlantPosition;

    [SerializeField] float WateringCanEffect;

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
        if (!isInventory)
        {
            switch (selectedTool)
            {
                // changing that Icon in the inventory to show your tool
                case 1:
                    myInventory.transform.parent.GetChild(1).gameObject.SetActive(true);
                    myInventory.transform.parent.GetChild(2).gameObject.SetActive(false);
                    myInventory.transform.parent.GetChild(3).gameObject.SetActive(false);
                    break;
                case 2:
                    myInventory.transform.parent.GetChild(1).gameObject.SetActive(false);
                    myInventory.transform.parent.GetChild(2).gameObject.SetActive(true);
                    myInventory.transform.parent.GetChild(3).gameObject.SetActive(false);
                    break;
                case 3:
                    myInventory.transform.parent.GetChild(1).gameObject.SetActive(false);
                    myInventory.transform.parent.GetChild(2).gameObject.SetActive(false);
                    myInventory.transform.parent.GetChild(3).gameObject.SetActive(true);
                    break;
            }
        }
        if (pickedUpPlant != null)
        {
            pickedUpPlant.transform.position = myCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        // making it so that the border disapears if the timer ends
        selectionTimer -= Time.deltaTime;
        if (selectionTimer <= 0 && !isInventory)
        {
            tileSelectBorder.position += new Vector3(1000, 1000, 0);
        }
        // flipping tools to the left
        if (Input.GetKeyDown(KeyCode.Q) && pickedUpPlant == null)
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
        // flipping tools to the right
        else if (Input.GetKeyDown(KeyCode.E) && pickedUpPlant == null)
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
        // decreasing use timer when you stop using a tool
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
        // using a tool or plant on a tile
        if (!isInventory)
        {
            // if you can plant a seed or picked up plant you will
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
                // uses the seed or plant on the tile if you have waited long enough
                if (useTimer > useTimerMax)
                {
                    // using a seed
                    if (pickedUpPlant == null)
                    {
                        selectedInventoryTile = myInventory.GetComponent<TileScript>().selectedInventoryTile;
                        myTilemap.SetTile(myTilePositionInt, selectedInventoryTile);
                        useTimer = 0;
                    }
                    // using a picked up plant
                    else
                    {
                        selectedInventoryTile = myInventory.GetComponent<TileScript>().selectedInventoryTile;
                        myTilemap.SetTile(myTilePositionInt, pickedUpPlantType);
                        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
                        pickedUpPlantPosition = CheckTile() * (int)transform.parent.GetComponent<Grid>().cellSize.x + new Vector3Int(1, 1, 0);
                        pickedUpPlant.transform.position = pickedUpPlantPosition;
                        pickedUpPlant.transform.GetChild(0).GetChild(0).GetComponent<CottonScript>().growing = true;
                        pickedUpPlant = null;
                        useTimer = 0;
                    }
                }
            }
            // using a hoe on untilled soil
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
            // using a watering can
            else if (selectedTool == 2)
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
                    // Seeing if you are hovering over a plant
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).transform.position == CheckTile()
                            * (int)transform.parent.GetComponent<Grid>().cellSize.x
                            + new Vector3Int(1, 1, 0))
                        {
                            transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<CottonScript>().time += WateringCanEffect;
                        }
                    }
                    useTimer = 0;
                }
            }
        }
    }
    private void OnMouseDown()
    {
        // using your shovel
        if (selectedTool == 3 && pickedUpPlant == null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                // seeing if you are hovering over a plant and if so, getting the position of the plant
                if (transform.GetChild(i).transform.position == CheckTile()
                    * (int)transform.parent.GetComponent<Grid>().cellSize.x
                    + new Vector3Int(1, 1, 0)
                    && pickedUpPlant == null)
                {
                    // picking up the plant
                    pickedUpPlant = Instantiate(transform.GetChild(i).gameObject);
                    pickedUpPlant.transform.parent = transform;

                    pickedUpPlant.transform.GetChild(0).GetChild(0).GetComponent<CottonScript>().time
                     = transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<CottonScript>().time;

                    pickedUpPlant.transform.GetChild(0).GetChild(0).GetComponent<CottonScript>().growing
                     = false;

                    pickedUpPlantType = myTilemap.GetTile<Tile>(CheckTile());
                    Debug.Log(pickedUpPlant.transform.position);
                    myTilemap.SetTile(CheckTile(), tilledSoil);
                    pickedUpPlant.transform.position += Vector3.one;

                    // killing the left over copy if necessary 
                    if (transform.GetChild(i).transform.position == CheckTile()
                    * (int)transform.parent.GetComponent<Grid>().cellSize.x
                    + new Vector3Int(1, 1, 0) && transform.GetChild(i) != pickedUpPlant)
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
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
