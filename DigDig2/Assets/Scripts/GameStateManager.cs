using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentGameState {  get; private set; }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChange;
    void Start()
    {
        
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
}
