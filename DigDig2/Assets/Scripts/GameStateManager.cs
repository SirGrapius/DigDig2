using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentGameState {  get; private set; }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChange;
    public float heldMoneyAmount;
    private static GameStateManager instance;
    public static GameStateManager Instance
    {
        get
        {
            if (!Application.isPlaying)
            {
                return null;
            }
            if (instance == null)
            {
                Debug.Log("what");
                Instantiate(Resources.Load<GameStateManager>("GameManager"));
            }

            return instance;
        }
    }

    public PlayerMovement Player { get; set; }
    public RoundManager RoundManager { get; set; }


    void Start()
    {
        if (SceneManager.loadedSceneCount == 0)
        {
            SetState(GameState.Gameplay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentGameState == GameState.Paused)
        {
            Rigidbody2D playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            playerRB.linearVelocity = Vector3.zero;
        }
    }

    private GameStateManager()
    {

    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
        {
            return;
        }

        CurrentGameState = newGameState;
        OnGameStateChange?.Invoke(newGameState);
    }

    private void OnApplicationQuit()
    {
        SaveSystem.Save();
    }
}
