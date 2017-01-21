using System.Collections;
using System.Linq;
﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    UIController UIController;

    [SerializeField]
    ParticleSystem deathParticlesPrefab;
    ParticleSystem deathParticles;
    [SerializeField]
    private GameObject[] otherThingsToDestroy;

    Rigidbody2D rb2d;

    SpriteRenderer[] sprites;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();

        deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        deathParticles.Stop();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            // Do trippy shit
            foreach (SpriteRenderer item in sprites)
            {
                item.enabled = false;
            }
            rb2d.simulated = false;
            deathParticles.transform.position = transform.position;
            deathParticles.Play();
            UIController.EndGame();
            foreach (GameObject o in otherThingsToDestroy)
            {
                Destroy(o);
            }
        }
    }
}