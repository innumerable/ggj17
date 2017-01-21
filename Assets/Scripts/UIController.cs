using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    private GameObject hiScoreUI;

    [SerializeField]
    private ScoreHandler scoreHandler;

    private bool playing = false;
    private bool takesInput = true;
    private bool finished = false;

    void Start()
    {
        startGameUI.SetActive(true);
        endGameUI.SetActive(false);
        restartPromptUI.SetActive(false);
        scoreUI.SetActive(false);
        hiScoreUI.SetActive(false);
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
        playing = true;
        Time.timeScale = 1f;
    }

    public void EndGame()
    {
        endGameUI.SetActive(true);
        hiScoreUI.SetActive(true);
        playing = false;
        finished = true;

        scoreHandler.AddHiscore();
        scoreHandler.ShowHiscores(hiScoreUI.GetComponentInChildren<Text>());
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