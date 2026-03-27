using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [SerializeField] bool isPaused;

    [SerializeField] GameObject canvas;
    [SerializeField] AudioSource source;
    [SerializeField] ScreenFade screenFader;

    [SerializeField] List<AudioClip> audioList;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] GameStateManager gsManager;

    [SerializeField] GameObject currentMenu;


    void Awake()
    {
        source = GetComponent<AudioSource>();
        gsManager = GetComponent<GameStateManager>();
        screenFader = GetComponentInChildren<ScreenFade>();
        StartCoroutine(screenFader.FadeInCoroutine(fadeDuration));
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SaveSystem.Save();
            gsManager.SetState(GameState.Paused);
        }

        if (gsManager.CurrentGameState == GameState.Paused && !isPaused)
        {
            StartCoroutine(PauseGame());
        }
        if (gsManager.CurrentGameState == GameState.Gameplay && isPaused)
        {
            StartCoroutine(UnpauseGame());
        }
    }

    public void WhatButton(string buttonName)
    {
        switch (buttonName)
        {
            case "Play":
                {
                    StartCoroutine(LoadScene("GameScene"));
                    break;
                }

            case "NewGame":
                {
                    UnityEngine.SceneManagement.Scene activeScene = SceneManager.GetActiveScene();
                    string sceneName = activeScene.name;
                    if (sceneName == "MainMenu")
                    {
                        MainMenu menu = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMenu>();
                    }
                    SaveSystem.ClearData();
                    StartCoroutine(LoadScene("GameScene"));
                    break;
                }

            case "Options":
                {
                    settingsMenu.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y, -5);
                    currentMenu = settingsMenu;
                    pauseMenu.transform.position = new Vector3(30000, 30000, 0);
                    break;
                }

            case "MainMenu":
                {
                    StartCoroutine(LoadScene("Main Menu"));
                    break;
                }

            case "Quit":
                {
                    source.clip = audioList[0];
                    source.Play();
                    StartCoroutine(QuitGame());
                    break;
                }
            case "Resume":
                {
                    currentMenu.transform.position = new Vector3(30000, 30000, 0);
                    if (currentMenu == settingsMenu)
                    {
                        currentMenu = pauseMenu;
                        pauseMenu.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y, -5);
                    }
                    else
                    {
                        screenFader.FadeCoroutine(new Color(255, 255, 255, 0.5f), new Color(255, 255, 255, 0), 0.25f);
                        gsManager.SetState(GameState.Gameplay);
                    } 
                    break;
                }
        }

    }

    public IEnumerator LoadScene(string sceneName)
    {
        if (gsManager.CurrentGameState == GameState.Paused)
        {
            StartCoroutine(screenFader.FadeCoroutine(new Color(0, 0, 0, 0.5f), new Color(0, 0, 0, 1f), 0.25f));
            yield return new WaitForSeconds(fadeDuration);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            StartCoroutine(screenFader.FadeOutCoroutine(fadeDuration));
            yield return new WaitForSeconds(fadeDuration);
            SceneManager.LoadScene(sceneName);
        }
    }

    public IEnumerator PauseGame()
    {
        StartCoroutine(screenFader.FadeCoroutine(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.5f), 0.25f));
        pauseMenu.transform.position = new Vector3(canvas.transform.position.x, canvas.transform.position.y, -5);
        SaveSystem.Save();
        yield return new WaitForSeconds(0.1f);
        currentMenu = pauseMenu;
        isPaused = true;
        yield return null;
    }

    public IEnumerator UnpauseGame()
    {
        StartCoroutine(screenFader.FadeCoroutine(new Color(255, 255, 255, 0.5f), new Color(255, 255, 255, 0f), 0.25f));
        pauseMenu.transform.position = new Vector3(10000,10000,0);
        yield return new WaitForSeconds(0.1f);
        isPaused = false;
        yield return null;
    }

    IEnumerator QuitGame()
    {
        source.clip = audioList[0];
        source.Play();
        StartCoroutine(screenFader.FadeOutCoroutine(fadeDuration));
        yield return new WaitForSeconds(fadeDuration);

        Application.Quit();
    }
}
