using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float fadeDuration;

    [SerializeField] GameObject canvas;
    [SerializeField] AudioSource source;
    [SerializeField] ScreenFade screenFader;

    [SerializeField] List<AudioClip> audioList;

    [SerializeField] GameObject pauseMenu;


    void Awake()
    {
        source = GetComponent<AudioSource>();

        screenFader = GetComponentInChildren<ScreenFade>();
    }

    void Update()
    {

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
                    //unpause paused objects
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
        //figure out how to pause everything other than scene loader and canvas
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
