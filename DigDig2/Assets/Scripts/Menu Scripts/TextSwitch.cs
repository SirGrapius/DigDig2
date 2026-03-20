using TMPro;
using UnityEngine;

public class TextSwitch : MonoBehaviour
{
    [SerializeField] string[] texts;
    [SerializeField] TextMeshProUGUI display;
    [SerializeField] int currect;
    [SerializeField] GameStateManager gsManager;

    private void Awake()
    {
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();
    }

    private void Start()
    {
        gsManager.SetState(GameState.Paused);
    }

    void Update()
    {
        display.text = texts[currect];   
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Next()
    {
        if (currect != texts.Length-1)
        {
            currect++;
        }
    }
    public void Previous()
    {
        if (currect != 0)
        {
            currect--;
        }
    }
    public void Close()
    {
        gsManager.SetState(GameState.Gameplay);
        Destroy(transform.parent.parent.gameObject);
    }
}
