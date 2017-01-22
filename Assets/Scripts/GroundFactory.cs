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
//        Equation[] equations = CreateRandomEquations(3, 2, 1);
        Equation[] equations = MakeEquations();

        ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.Initialise(equations);
        end = segmentWidth;

        MakeAMeshBoii(-segmentWidth);
        GameObject groundHolder = MakeAMeshBoii(0f);

        MovePlayerAboveGround(groundHolder);
    }

    Equation[] MakeEquations()
    {
        Equation[] waves = new Equation[6];
        // Base oscillations
        waves[0] = new Equation(3, 0.0001f, .03f, 0, Mathf.PI, -0.1f);

        // Very wide, increasing amplitudes and offsets
        waves[1] = new Equation(0.4f, 0.007f, 0.21f, 0, Mathf.PI);
        waves[2] = new Equation(0.3f, 0.006f, 0.13f, 0, Mathf.PI / 2);
        waves[3] = new Equation(0.2f, 0.005f, 0.05f, 0, Mathf.PI / 3);
        waves[4] = new Equation(0.2f, 0.004f, 0.09f, 0, Mathf.PI / 4);

        // Frequent but very low ampl
        waves[5] = new Equation(0f, 0.0005f, 0.6f);

        return waves;
    }

    Equation[] CreateRandomEquations(int wideWaves, int midWaves, int thinWaves)
    {
        Equation[] waves = new Equation[1 + (3 * (wideWaves + midWaves + thinWaves))];

        int i = 1;
    
        // Base wave
        waves[0] = new Equation(2, 0.0001f, .05f, -.0005f, Mathf.PI, -0.1f);

        // Very wide, mid amplitude
        for (; i < 1 + wideWaves; i++)
        {
            Equation eq = new Equation(Random.Range(0.02f, 0.05f),
                                    Random.Range(0.0002f, 0.0005f),
                                    Random.Range(0.0002f, 0.0005f),
                                    0,
                                    0,
                                    0f);
            waves[i] = eq;
            Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}", eq.A, eq.Ax, eq.B, eq.Bx, eq.C));
        }

        // Middling width, Middling amplitude
        for (; i < 1 + wideWaves + midWaves; i++)
        {
            Equation eq = new Equation(Random.Range(0.02f, 0.05f),
                                    Random.Range(0.0002f, 0.0005f),
                                    Random.Range(0.02f, 0.05f),
                                    0,
                                    0,
                                    0f);
            waves[i] = eq;
            Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}", eq.A, eq.Ax, eq.B, eq.Bx, eq.C));
        }

        // Reasonably thin, low amplitude
        for (; i < 1 + wideWaves + midWaves + thinWaves; i++)
        {
            Equation eq = new Equation(Random.Range(0.002f, 0.005f),
                                    0,
                                    Random.Range(0.1f, 0.3f),
                                    0,
                                    0,
                                    0f);
            waves[i] = eq;
            Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}", eq.A, eq.Ax, eq.B, eq.Bx, eq.C));
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

        if (Camera.main.transform.position.x >= end - camWidth * 2)
        {
            MakeAMeshBoii(end);
            end += segmentWidth;
        }
    }
}
