using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] RoundManager roundManager;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] TileScript tileScript;
    [SerializeField] TextMeshPro dialogbox;
    [SerializeField] int currentDialogText;
    [SerializeField] string[] tutorialText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (roundManager.tutorialDone == false)
        {
            roundManager.gameObject.SetActive(false);
        }
        else 
        { 
            dialogbox.gameObject.SetActive(false); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (roundManager.gameObject.activeSelf == false)
        {
            dialogbox.text = tutorialText[currentDialogText];
            switch (currentDialogText)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.N)) { currentDialogText++; }
                    if (Input.GetKeyDown(KeyCode.Y))
                    {
                        roundManager.gameObject.SetActive(true);
                        gameObject.SetActive(false);
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) { currentDialogText++; }
                    break;
                case 9:
                    if (Input.GetKeyDown(KeyCode.Mouse0) && tileScript.selectedTool == 3 && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) { currentDialogText++; }
                    break;
                case 10:
                    if (Input.GetKeyDown(KeyCode.Space) && tileScript.selectedTool == 3) { currentDialogText++; }
                    break;
                case 16:
                    sceneLoader.WhatButton("NewGame");
                    break;
                default:
                    if (Input.GetKeyDown(KeyCode.Mouse0)) { currentDialogText++; }
                    break;

            }
        }
    }
}
