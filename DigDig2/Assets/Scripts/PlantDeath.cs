using UnityEngine;

public class PlantDeath : MonoBehaviour
{
    [SerializeField] GameObject daddy;
    [SerializeField] int hp;

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
        daddy.transform.parent.GetComponent<TileScript>().myTilemap.SetTile(Vector3Int.FloorToInt(transform.position / transform.parent.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x), transform.parent.GetComponent<TileScript>().unTilledSoil);
        Destroy(daddy);
    }
}
