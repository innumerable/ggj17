using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject startGameUI;

    [SerializeField]
    private GameObject endGameUI;

    [SerializeField]
    private GameObject restartPromptUI;

    [SerializeField]
    private GameObject scoreUI;

    private bool playing = false;
    private bool takesInput = true;
    private bool finished = false;

    void Start()
    {
        startGameUI.SetActive(true);
        endGameUI.SetActive(false);
        restartPromptUI.SetActive(false);
        scoreUI.SetActive(false);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (finished && takesInput && PlayerInput.IsPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (!playing && takesInput && PlayerInput.IsPressed)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        startGameUI.SetActive(false);
        endGameUI.SetActive(false);
        restartPromptUI.SetActive(false);
        scoreUI.SetActive(true);
        Time.timeScale = 1f;
        playing = true;
    }

    public void EndGame()
    {
        endGameUI.SetActive(true);
        Time.timeScale = 0f;
        playing = false;
        finished = true;
        StartCoroutine(WaitBeforeInput(1f));
    }

    IEnumerator WaitBeforeInput(float time)
    {
        takesInput = false;
        yield return new WaitForSecondsRealtime(time);
        takesInput = true;
        restartPromptUI.SetActive(true);
    }
}