using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject player;

	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
        // We want to retain the Z position of the camera.
        Vector3 newPos = new Vector3(
                                    player.transform.position.x,
                                    player.transform.position.y,
                                    this.transform.position.z);

        this.transform.position = newPos;
	}
}
