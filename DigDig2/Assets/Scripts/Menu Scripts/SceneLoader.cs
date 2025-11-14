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
                    StartCoroutine(LoadScene("Options"));
                    break;
                }

            case "MainMenu":
                {
                    StartCoroutine(LoadScene("AwesomeSauceMenu"));
                    break;
                }

            case "Quit":
                {
                    source.clip = audioList[0];
                    source.Play();
                    StartCoroutine(QuitGame());
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

    IEnumerator QuitGame()
    {
        source.clip = audioList[0];
        source.Play();
        screenFader.FadeOutCoroutine(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        Application.Quit();
    }
}
