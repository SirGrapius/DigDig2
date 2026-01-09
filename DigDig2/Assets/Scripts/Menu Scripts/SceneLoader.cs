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
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
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

            case "Options":
                {
                    settingsMenu.transform.position = canvas.transform.position;
                    currentMenu = settingsMenu;
                    pauseMenu.transform.position = new Vector3(30000, 30000, 0);
                    break;
                }

            case "MainMenu":
                {
                    StartCoroutine(LoadScene("MainMenu"));
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
                    Time.timeScale = 1;
                    if (currentMenu == settingsMenu)
                    {
                        currentMenu = pauseMenu;
                        pauseMenu.transform.position = canvas.transform.position;
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
        source.clip = audioList[0];
        source.Play();
        screenFader.FadeOutCoroutine(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);


        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator PauseGame()
    {
        screenFader.FadeCoroutine(new Color(255, 255, 255, 0), new Color(255, 255, 255, 0.5f), 0.25f);
        pauseMenu.transform.position = canvas.transform.position;
        yield return new WaitForSeconds(0.1f);
        currentMenu = pauseMenu;
        isPaused = true;
        yield return null;
    }

    public IEnumerator UnpauseGame()
    {
        screenFader.FadeCoroutine(new Color(255, 255, 255, 0.5f), new Color(255, 255, 255, 0f), 0.25f);
        pauseMenu.transform.position = new Vector3(10000,10000,0);
        yield return new WaitForSeconds(0.1f);
        isPaused = false;
        yield return null;
    }

    IEnumerator QuitGame()
    {
        source.clip = audioList[0];
        source.Play();
        screenFader.FadeOutCoroutine(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        Application.Quit();
    }
}
