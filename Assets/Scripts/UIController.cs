using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject startGameUI;

    private bool playing = false;

    void Start()
    {
        startGameUI.SetActive(true);
        Time.timeScale = 0f;
    }
	
	void Update()
    {
        if (PlayerInput.IsPressed)
        {
            startGameUI.SetActive(false);
            Time.timeScale = 1f;
        }
	}
}