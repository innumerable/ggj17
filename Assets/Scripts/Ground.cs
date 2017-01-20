using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
public class Ground : MonoBehaviour
{
    private float end;

    public void Initialise(float start, float width, int iterations)
    {
        CreateMesh(start, width, iterations);
        this.end = start + width;
    }

    void CreateMesh(float start, float width, int iterations)
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.numPositions = iterations + 1;

        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] colliderPoints = new Vector2[iterations + 1];
        
        float gap = width / iterations;
        
        colliderPoints[0] = new Vector3(start, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(start));
        line.SetPosition(0, colliderPoints[0]);

        for (int i = 0; i < iterations; i++)
        {
            float x = start + gap * (i + 1);
            colliderPoints[i + 1] = new Vector3(x, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(x));
            line.SetPosition(i + 1, colliderPoints[i + 1]);
        }
        edgeCollider.points = colliderPoints;
    }

    void Update()
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        if (Camera.main.transform.position.x > end + camWidth)
        {
            Destroy(gameObject);
        }
    }
}
