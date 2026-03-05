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
        Destroy(transform.parent.parent.gameObject);
    }
}
