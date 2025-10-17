using UnityEngine;

public class PlantDeath : MonoBehaviour
{
    [SerializeField] GameObject daddy;
    int hp;

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
        daddy.transform.parent.GetComponent<TileScript>().myTilemap.SetTile(Vector3Int.FloorToInt(daddy.transform.position / daddy.transform.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x), daddy.transform.parent.GetComponent<TileScript>().tilledSoil);
        Destroy(daddy);
    }
}
