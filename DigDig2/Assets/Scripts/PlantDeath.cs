using UnityEngine;

public class PlantDeath : MonoBehaviour
{
    [SerializeField] GameObject daddy;
    [SerializeField] GameObject child;
    [SerializeField] AnimationClip deathAnim;
    [SerializeField] Animator myAnimator;
    [SerializeField] int hp;
    
    float time;

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
        myAnimator.SetBool("Death", true);
        if (child != null)
        {
            Destroy(child);
        }   
        if (time >= deathAnim.length)
        {
            Destroy(daddy);
        }
    }
    void Update()
    {
        if (myAnimator.GetBool("Death"))
        {
            time += Time.deltaTime;
        }
    }
}
