﻿using UnityEngine;

class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    UIController UIController;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            // Do trippy shit
            UIController.EndGame();
        }
    }
}