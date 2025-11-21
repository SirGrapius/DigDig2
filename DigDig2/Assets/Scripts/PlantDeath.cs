using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantDeath : MonoBehaviour
{
    [SerializeField] GameObject daddy;
    [SerializeField] int hp;
    public RuleTile tileUnderneath;

    public void Damage(int damageValue)
    {
        hp -= damageValue;
        if (hp <= 0)
        {
            Decay();
        }
    }
    public void Decay()
    {
        daddy.transform.parent.GetComponent<TileScript>().myTilemap.SetTile(Vector3Int.FloorToInt(transform.position / transform.parent.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x), tileUnderneath);
        Destroy(daddy);
    }
}
