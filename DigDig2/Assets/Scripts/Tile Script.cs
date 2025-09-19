using UnityEngine;
using UnityEngine.Tilemaps;

public class TileScript : MonoBehaviour
{
    [SerializeField] Tilemap myTilemap;
    [SerializeField] TilemapCollider2D myCollider;
    [SerializeField] Camera myCamera;
    [SerializeField] Vector3Int myTilePosition;
    [SerializeField] Tile myTile;
    [SerializeField] Transform tileSelectBorder;
    [SerializeField] int SelectionTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        myTilemap = GetComponent<Tilemap>();
        myCollider = GetComponent<TilemapCollider2D>();
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        SelectionTimer--;
        if (SelectionTimer <= 0)
        {
            tileSelectBorder.position += new Vector3(1000, 1000, 0);
        }
    }

    private void OnMouseOver()
    {
        myTilePosition = Vector3Int.FloorToInt(myCamera.ScreenToWorldPoint(Input.mousePosition));
        myTilePosition.z = 0;
        //myTilemap.SetTile(myTilePosition, myTile);
        Debug.Log(myTilemap.GetTile(myTilePosition));
        tileSelectBorder.position = myTilePosition;
        SelectionTimer = 5;
    }
}
