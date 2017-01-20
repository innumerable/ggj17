using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    Vector2 offset;

    [SerializeField]
    float smoothTime;

    // We have no rigidbody, so calculate velocity manually.
    Vector3 previousPosition;
    Vector3 velocity;

    void Start()
    {
        previousPosition = transform.position;
    }
	
	void FixedUpdate()
    {
        velocity = (transform.position - previousPosition) / Time.deltaTime;

        // We want to retain the Z position of the camera.
        Vector3 destination = new Vector3(
                                    player.transform.position.x,
                                    player.transform.position.y,
                                    this.transform.position.z) 
                                    + (Vector3)offset;

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);

        previousPosition = transform.position;
	}
}
