using System.IO;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string gameData;

    [SerializeField] ButtonInflation continueButton;
    void Start()
    {
        File.ReadAllText(SaveSystem.SaveFileName());
        gameData = File.ReadAllText(SaveSystem.SaveFileName());
    }

    void Update()
    {
        if (gameData == null)
        {
            continueButton.enabled = true;
        }
        else
        {
            continueButton.enabled = false;
        }
    }
}