using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float minDistance = 10f;
    [SerializeField]
    private float speed = 5f;

	void Update()
    {
        if (player.transform.position.x - transform.position.x > minDistance)
            transform.position = new Vector3(player.transform.position.x - minDistance + 0.5f, player.transform.position.y);

        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, player.transform.position.y);
	}
}
