using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AdvancedCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private LayerMask layers;
    [SerializeField]
    private float minSize = 10f;
    [SerializeField]
    private float maxSize = 40f;
    [SerializeField]
    private float snapSpeed = 15f;
    [SerializeField]
    private float scaleFactor = 1f;
    [SerializeField]
    [Range(-0.5f, 0.5f)]
    private float offsetXPercent;
    [SerializeField]
    [Range(-0.5f, 0.5f)]
    private float offsetYPercent;
    [SerializeField]
    private float cameraScaleFactor;

    private Camera cam;
    private Rigidbody2D playerRb;
    private float[] velocities = new float[30];
    private int velIndex = 0;

    private float[] distances = new float[30];
    private int distIndex = 0;

	void Start()
	{
	    cam = GetComponent<Camera>();
	    transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
	    playerRb = playerTransform.GetComponent<Rigidbody2D>();
	}
	
	void Update()
	{
	    List<float> heights = new List<float>();

	    for (int i = 0; i < 3; i++)
	    {
	        float angle = i*15f;
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.down, 100f, layers);
            Debug.DrawRay(playerTransform.position, Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.down);
	        if (hit.collider != null)
	        {
                float distance = playerTransform.position.y - hit.point.y;
                heights.Add(distance);
            }
        }

	    float thisFrameAvg = heights.Average();
	    distances[distIndex++%distances.Length] = thisFrameAvg;

        cam.orthographicSize = Mathf.Clamp(Mathf.Lerp(cam.orthographicSize, distances.Average() * cameraScaleFactor, Time.deltaTime * snapSpeed), minSize, maxSize);

        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

	    velocities[velIndex++%velocities.Length] = playerRb.velocity.magnitude;
	    float averageVel = velocities.Average();
	    float newX = playerTransform.position.x + Mathf.Clamp(averageVel * scaleFactor, -camWidth/4f, camWidth/8f);
        transform.position = new Vector3(newX + camWidth * offsetXPercent, playerTransform.position.y + camHeight * offsetYPercent, -10f);
	}
}
