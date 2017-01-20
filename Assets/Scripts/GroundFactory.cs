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
        //Equation[] equations = new[]
        //{
        //    new Equation(1f, .5f, 0f, 0f),
        //    new Equation(0.1f, .05f, 3f, 0f),
        //    new Equation(0.05f, 1f, 0f, 0f),
        //    new Equation(4f, .01f, 0f, 0f),
        //};

        Equation[] equations = CreateRandomEquations(10, 0f);

        ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.Initialise(equations);
        end = segmentWidth;

        MakeAMeshBoii(-segmentWidth);
        GameObject groundHolder = MakeAMeshBoii(0f);

        MovePlayerAboveGround(groundHolder);
    }

    Equation[] CreateRandomEquations(int waveCountPerType, float verticalOffset)
    {
        Equation[] waves = new Equation[1 + (3 * waveCountPerType)];

        int i = 0;

        float B = Random.Range(0.001f, 0.01f);
        //waves[0] = new Equation(1, Random.Range(20f, 50f), B, Mathf.PI, 0);
        waves[0] = new Equation(2, 0.0001f, .05f, .0005f, Mathf.PI);

        // Wide, middling amplitude
        for (; i < 1 + waveCountPerType; i++)
        {
            waves[i] = new Equation(Random.Range(0.25f, 0.75f), 
                                    Random.Range(0.001f, 0.005f), 
                                    Random.Range(0.01f, 0.02f),
                                    Random.Range(0f, 0.001f),
                                    0);
        }

        for (; i < 1 + 2 * waveCountPerType; i++)
        {
            waves[i] = new Equation();
        }

        for (; i < 1 + 3 * waveCountPerType; i++)
        {
            waves[i] = new Equation();
        }

        // Middling width, Middling amplitude

        // Reasonably thin, low amplitude

        for (i = 1; i < waveCountPerType; i++)
        {
            //waves[i] = new Equation(1, Random.Range(-.5f, 1f), Random.Range(0.01f, 1f), Random.Range(0f, 10f), 0f);
        }

        return waves;
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
