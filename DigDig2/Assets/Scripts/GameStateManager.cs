using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentGameState {  get; private set; }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChange;
    public float heldMoneyAmount;
    public TextMeshPro moneyUI;
    private static GameStateManager instance;

    [Header("Settings")]
    [SerializeField] public float sfxVolume;
    [SerializeField] public float musicVolume;
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

    private void Awake()
    {
        instance = this;
        if (SceneManager.GetSceneByName("Main Menu") != SceneManager.GetActiveScene())
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            RoundManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoundManager>();
        }
    }
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
        if (CurrentGameState == GameState.Paused && SceneManager.GetSceneByName("MainMenu") != SceneManager.GetActiveScene())
        {
            Rigidbody2D playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            playerRB.linearVelocity = Vector3.zero;
        }
        

        //Debug.Log(Player.ToString());
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
        Debug.Log("saved");
    }

    public void Save(ref SoundData sfxData, MoneyData moneyData)
    {
        moneyData.money = heldMoneyAmount;
        sfxData.sfxVol = sfxVolume;
        sfxData.musicVol = musicVolume;
    }

    public void Load(SoundData sfxData, MoneyData moneyData)
    {
        sfxVolume = sfxData.sfxVol;
        musicVolume = sfxData.musicVol;
        heldMoneyAmount = moneyData.money;
        moneyUI.text = ((int)moneyData.money).ToString();
    }
}



[System.Serializable]
public struct MoneyData
{
    public float money;
}

[System.Serializable]
public struct SoundData
{
    public float sfxVol;
    public float musicVol;
}
