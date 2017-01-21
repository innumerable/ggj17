using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject startGameUI;

    [SerializeField]
    private GameObject scoreUI;

    private bool playing = false;

    void Start()
    {
        startGameUI.SetActive(true);
        scoreUI.SetActive(false);
        Time.timeScale = 0f;
    }
	
	void Update()
    {
        if (!playing && PlayerInput.IsPressed)
        {
            startGameUI.SetActive(false);
            scoreUI.SetActive(true);
            Time.timeScale = 1f;
        }
	}
}