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

    private float end = 0f;

    void Start()
    {
        Equation[] equations = new[]
        {
            new Equation(1f, .5f, 0f, 0f)
        };
        ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.Initialise(equations);
        end = segmentWidth;
        MakeAMeshBoii(0f);
    }

    void MakeAMeshBoii(float start)
    {
        GameObject ground = Instantiate(groundPrefab);
        ground.transform.position = Vector3.zero;
        ground.GetComponent<Ground>().Initialise(start, segmentWidth, (int)segmentWidth * resolution);
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
