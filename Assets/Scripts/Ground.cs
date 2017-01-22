using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
public class Ground : MonoBehaviour
{
    [Header("Boosts")]
    [SerializeField]
    private float boostChance = 0.5f;
    [SerializeField]
    private float boostHeightMin = 5f;
    [SerializeField]
    private float boostHeightMax = 30f;
    [SerializeField]
    private GameObject[] boostPrefabs;

    private float end;

    private LineRenderer line;
    private EdgeCollider2D edgeCollider;

    public Vector2[] Points
    {
        get
        {
            return edgeCollider.points;
        }
        set
        {
            edgeCollider.points = value;
            line.SetPositions(value.Select(p => (Vector3)p).ToArray());
        }
    }

    public void Initialise(float start, float width, int iterations)
    {
        CreateMesh(start, width, iterations);
        this.end = start + width;
        if (Random.value < boostChance)
            PlaceBoost();
    }

    void PlaceBoost()
    {
        Vector2 position = Points[Random.Range(0, Points.Length - 1)] + Vector2.up * Random.Range(boostHeightMin, boostHeightMax);
        Instantiate(boostPrefabs[Random.Range(0, boostPrefabs.Length)], position, Quaternion.identity);
    }

    void CreateMesh(float start, float width, int iterations)
    {
        LineRenderer newLine = GetComponent<LineRenderer>();
        newLine.numPositions = iterations + 1;

        EdgeCollider2D newEdgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] colliderPoints = new Vector2[iterations + 1];
        
        float gap = width / iterations;
        
        colliderPoints[0] = new Vector3(start, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(start));
        newLine.SetPosition(0, colliderPoints[0]);

        for (int i = 0; i < iterations; i++)
        {
            float x = start + gap * (i + 1);
            colliderPoints[i + 1] = new Vector3(x, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(x));
            newLine.SetPosition(i + 1, colliderPoints[i + 1]);
        }
        newEdgeCollider.points = colliderPoints;

        line = newLine;
        edgeCollider = newEdgeCollider;
    }

    void Update()
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        if (Camera.main.transform.position.x > end + camWidth * 2)
        {
            Destroy(gameObject);
        }
    }
}
