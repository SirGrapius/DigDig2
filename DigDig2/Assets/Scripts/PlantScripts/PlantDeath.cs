using UnityEngine;

public class PlantDeath : MonoBehaviour
{
    [SerializeField] GameObject daddy;
    [SerializeField] GameObject child;
    [SerializeField] AnimationClip deathAnim;
    [SerializeField] Animator myAnimator;
    public int hp;
    public int hpMax;
    public bool decaying;
    
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
        if (hp <= 0)
        {
            Decay();
        }
    }
    public void Decay()
    {
        daddy.transform.parent.GetComponent<TileScript>().myTilemap.SetTile
            (Vector3Int.FloorToInt(transform.position / transform.parent.parent.GetComponent<Grid>().cellSize.x * transform.localScale.x), 
            transform.parent.GetComponent<TileScript>().unTilledSoil);
        myAnimator.SetBool("Death", true);
        decaying = true;
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
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
    void OnDestroy()
    {
        gsManager.OnGameStateChange -= OnGameStateChanged;
    }
}
