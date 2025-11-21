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

    [SerializeField] GameStateManager gsManager;


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
            GameState currentGameState = gsManager.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;

            gsManager.SetState(newGameState);
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
                    //brings up a menu
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
                    pauseMenu.transform.position = new Vector3(10000, 10000, 0);
                    Time.timeScale = 1;
                    screenFader.FadeCoroutine(new Color(255, 255, 255, 0.5f), new Color(255, 255, 255, 0), 0.25f);
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
        isPaused = true;
        Time.timeScale = 0;
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
