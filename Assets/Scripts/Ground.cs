using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(EdgeCollider2D))]
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
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] colliderPoints = new Vector2[iterations + 1];

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[iterations * 2 + 2];
        int[] triangles = new int[iterations * 6];
        float gap = width / iterations;

        vertices[0] = new Vector3(start, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(start));
        vertices[1] = new Vector3(start, vertices[0].y - 1f);
        colliderPoints[0] = vertices[0];

        for (int i = 0; i < iterations; i++)
        {
            float x = start + gap * (i + 1);
            vertices[2 * i + 2] = new Vector3(x, ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.GetHeight(x));
            vertices[2 * i + 3] = new Vector3(x, vertices[2 * i + 2].y - 1f);
            colliderPoints[i + 1] = vertices[2 * i + 2];
            // First triangle
            triangles[6 * i] = i * 2;
            triangles[6 * i + 1] = i * 2 + 2;
            triangles[6 * i + 2] = i * 2 + 1;
            // Second triangle
            triangles[6 * i + 3] = i * 2 + 1;
            triangles[6 * i + 4] = i * 2 + 2;
            triangles[6 * i + 5] = i * 2 + 3;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        edgeCollider.points = colliderPoints;
    }

    void Update()
    {
        if (Camera.main.transform.position.x > end + 10f)
        {
            Destroy(gameObject);
        }
    }
}
