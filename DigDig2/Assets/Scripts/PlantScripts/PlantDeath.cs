using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantDeath : MonoBehaviour
{
    [SerializeField] GameObject daddy;
    [SerializeField] GameObject child;
    [SerializeField] AnimationClip deathAnim;
    [SerializeField] Animator myAnimator;
    public int hp;
    public int hpMax;
    public bool decaying;
    [SerializeField] bool chili;
    
    float time;
    public RuleTile tileUnderneath;
    public RuleTile grassUnderneath;

    [SerializeField] GameStateManager gsManager;

    private void Awake()
    {
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    private void Start()
    {
        gsManager.OnGameStateChange += OnGameStateChanged;
    }

    public void Damage(int damageValue)
    {
        hp -= damageValue;
        if (hp <= 0 && !chili)
        {
            Decay(false);
        }
    }
    public void Decay(bool isChili)
    {
        transform.parent.parent.GetChild(0).GetComponent<TileScript>().myTilemap.SetTile
            (Vector3Int.FloorToInt(new Vector3
            (transform.position.x / transform.parent.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x,
            transform.position.y / transform.parent.parent.GetComponent<Grid>().cellSize.y * transform.localScale.x, 0)),
            tileUnderneath);
        transform.parent.GetComponent<Tilemap>().SetTile
            (Vector3Int.FloorToInt(new Vector3
            (transform.position.x / transform.parent.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x,
            transform.position.y / transform.parent.parent.GetComponent<Grid>().cellSize.y * transform.localScale.x, 0)),
            null);
        Debug.Log((Vector3Int.FloorToInt(new Vector3
            (transform.position.x / transform.parent.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x,
            transform.position.y / transform.parent.parent.GetComponent<Grid>().cellSize.y * transform.localScale.x, 0))));
        myAnimator.SetBool("Death", true);
        decaying = true;
        if (child != null)
        {
            Destroy(child);
        }   
        if (isChili)
        {
            Destroy(daddy);
        }
    }
    void Update()
    {
        if (myAnimator.GetBool("Death"))
        {
            time += Time.deltaTime;
            if (time >= deathAnim.length)
            {
                Destroy(daddy);
            }
        }
        
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }
}
