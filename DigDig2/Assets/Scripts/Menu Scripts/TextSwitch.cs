using TMPro;
using UnityEngine;

public class TextSwitch : MonoBehaviour
{
    [SerializeField] string[] texts;
    [SerializeField] TextMeshProUGUI display;
    [SerializeField] int currect;
    void Update()
    {
        display.text = texts[currect];   
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Next();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Destroy(transform.parent.parent);
        }
    }

    void Next()
    {
        if (currect != texts.Length-1)
        {
            currect++;
        }
    }
    void Previous()
    {
        if (currect != 0)
        {
            currect--;
        }
    }
}
