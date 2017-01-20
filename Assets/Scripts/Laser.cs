using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float minDistance = 10f;
    [SerializeField]
    private float speed = 5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

	void Update()
    {
        if (player.transform.position.x - transform.position.x > minDistance)
            transform.position = new Vector3(player.transform.position.x - minDistance + 0.5f, player.transform.position.y);

        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, player.transform.position.y);

        Camera.main.backgroundColor = Color.Lerp(new Color(0.5f, 0f, 0f, 1f), Color.black, (player.transform.position.x - transform.position.x) / (minDistance / 3f));
        audioSource.volume = Mathf.Lerp(1, 0, (player.transform.position.x - transform.position.x) / (minDistance / 1.5f));
    }
}
