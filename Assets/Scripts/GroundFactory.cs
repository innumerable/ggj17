using System.Collections.Generic;
using System.Threading;
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

    /// <summary>
    /// How many Ground objects are cached.
    /// </summary>
    [SerializeField]
    private int cacheCount;

    Thread workerThread;
    
    private Queue<Vector2[]> cache = new Queue<Vector2[]>();

    private float end = 0f;
    private float endAsync = 0f;

    void Start()
    {
        Equation[] equations = CreateRandomEquations(20, 0f);

        ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.Initialise(equations);
        end = segmentWidth;
        endAsync = segmentWidth;

        MakeAMeshBoii(-segmentWidth);
        GameObject groundHolder = MakeAMeshBoii(0f);

        MovePlayerAboveGround(groundHolder);

        workerThread = new Thread(CreateGroundPointsIntoCache);
        workerThread.Start();
    }

    Equation[] CreateRandomEquations(int waveCountPerType, float verticalOffset)
    {
        Equation[] waves = new Equation[1 + (3 * waveCountPerType)];

        int i = 1;

        waves[0] = new Equation(2, 0.00001f, .05f, -.0005f, Mathf.PI, -0.25f);

        // Wide, middling amplitude
        for (; i < 1 + waveCountPerType; i++)
        {
            waves[i] = new Equation(Random.Range(0.05f, 0.25f),
                                    Random.Range(0.00001f, 0.00005f),
                                    Random.Range(0.0001f, 0.0002f),
                                    -Random.Range(0f, 0.001f),
                                    0,
                                    0f);
        }

        // Middling width, Middling amplitude
        for (; i < 1 + 2 * waveCountPerType; i++)
        {
            waves[i] = new Equation(Random.Range(0.05f, 0.2f),
                                    Random.Range(0.0001f, 0.0005f),
                                    Random.Range(0.0001f, 0.00002f),
                                    -Random.Range(0f, 0.001f),
                                    0,
                                    0);
        }

        // Reasonably thin, low amplitude
        for (; i < 1 + 3 * waveCountPerType; i++)
        {
            waves[i] = new Equation(Random.Range(0.003f, 0.015f),
                                    Random.Range(0.00001f, 0.00005f),
                                    Random.Range(0.002f, 0.004f),
                                    -Random.Range(0f, 0.001f),
                                    0,
                                    0);
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
            //MakeAMeshBoii(end);
            CreateGroundPointsIntoCache();
            InstantiateFromCache();
            end += segmentWidth;
        }

        if (cache.Count < cacheCount &&
            workerThread.ThreadState == ThreadState.Stopped)
        {
            workerThread = new Thread(CreateGroundPointsIntoCache);
            workerThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            workerThread.Start();
        }
    }

    GameObject InstantiateFromCache()
    {
        GameObject newGround = Instantiate(groundPrefab);
        Vector2[] points = cache.Dequeue();
        newGround.GetComponent<Ground>().Initialise(points, points[0].x, segmentWidth);

        return newGround;
    }
    
    void CreateGroundPointsIntoCache()
    {
        //endAsync, segmentWidth, iterations
        int iterations = (int)segmentWidth * resolution;

        Vector2[] points = new Vector2[iterations + 1];

        float gap = segmentWidth / iterations;

        points[0] = new Vector3(endAsync, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(endAsync));

        for (int i = 0; i < iterations; i++)
        {
            float x = endAsync + gap * (i + 1);
            points[i + 1] = new Vector3(x, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(x));
        }

        endAsync += segmentWidth;

        cache.Enqueue(points);

        Debug.Log(string.Format("I made a ground! There's {0} now.", cache.Count));
    }
}
