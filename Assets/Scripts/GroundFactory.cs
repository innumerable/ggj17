using System.Linq;
using UnityEngine;

class GroundFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject groundPrefab;
    [SerializeField]
    private float segmentWidth = 10f;
    [SerializeField]
    private int resolution = 10;

    [SerializeField]
    private GameObject[] playerObjectGroup;
    /// <summary>
    /// This should be in degrees.
    /// </summary>
    [SerializeField]
    private float angleOfSlopeToPlacePlayerAbove = -10f;

    [SerializeField]
    private float distanceAboveGround = 5f;

    private float end = 0f;

    void Start()
    {
        // TODO randomise this shit
        Equation[] equations = new[]
        {
            new Equation(1f, .5f, 0f, 0f),
            new Equation(0.1f, .05f, 3f, 0f),
            new Equation(0.05f, 1f, 0f, 0f),
            new Equation(4f, .01f, 0f, 0f),
        };
        ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.Initialise(equations);
        end = segmentWidth;

        MakeAMeshBoii(-segmentWidth);
        GameObject groundHolder = MakeAMeshBoii(0f);

        MovePlayerAboveGround(groundHolder);
    }

    void MovePlayerAboveGround(GameObject groundHolder)
    {
        Ground ground = groundHolder.GetComponent<Ground>();
        Vector2[] points = ground.Points;

        for (int i = 1; i < points.Length; i++)
        {
            Vector2 difference = points[i] - points[i - 1];
            float gradient = Mathf.Asin(difference.y / difference.x) * Mathf.Rad2Deg;

            if (gradient < angleOfSlopeToPlacePlayerAbove)
            {
                // Two components: A point somewhere above the point on the line we want to place the player,
                // and the player's current Z position.
                Vector3 playerDestination = (Vector3)points[i] + (distanceAboveGround * Vector3.up) + 
                                            playerObjectGroup[0].transform.position.z * Vector3.forward;

                // Find the offset of each item from the first item: Like the offset of eyes from player.
                Vector3 firstPosition = playerObjectGroup[0].transform.position;
                Vector3[] offsets = new Vector3[playerObjectGroup.Length];

                // Move everything to a point above the ground.
                for (int j = 0; j < playerObjectGroup.Length; j++)
                {
                    Vector3 offset = playerObjectGroup[j].transform.position - firstPosition;

                    playerObjectGroup[j].transform.position = playerDestination + offset;
                }
                break;
            }
        }
    }

    GameObject MakeAMeshBoii(float start)
    {
        GameObject ground = Instantiate(groundPrefab);
        ground.transform.position = Vector3.zero;
        ground.GetComponent<Ground>().Initialise(start, segmentWidth, (int)segmentWidth * resolution);

        return ground;
    }

    void Update()
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize*2f;
        float camWidth = camHeight*cam.aspect;

        if (Camera.main.transform.position.x >= end - camWidth)
        {
            MakeAMeshBoii(end);
            end += segmentWidth;
        }
    }
}
