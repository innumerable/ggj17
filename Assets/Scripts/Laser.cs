using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float thresholdDistance = 48f;
    [SerializeField]
    private float minSpeed = 6f;
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaceLater());
    }

    IEnumerator PlaceLater()
    {
        transform.position = player.transform.position + Vector3.left * thresholdDistance * 6;
        yield return new WaitForSeconds(0.1f);
        transform.position = player.transform.position + Vector3.left * thresholdDistance * 6;
    }

	void Update()
	{
	    float distance = player.transform.position.x - transform.position.x;
	    float speed = distance < thresholdDistance ? minSpeed : Mathf.LerpUnclamped(minSpeed, minSpeed * 2f, distance - thresholdDistance);
//	    float speed = minSpeed;

        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, player.transform.position.y);

        // Visuals and audio
        Camera.main.backgroundColor = Color.Lerp(new Color(0.5f, 0f, 0f, 1f), Color.black, (player.transform.position.x - transform.position.x) / (thresholdDistance / 3f));
        audioSource.volume = Mathf.Lerp(1, 0, (player.transform.position.x - transform.position.x) / (thresholdDistance / 1.5f));
    }
}
