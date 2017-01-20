using System.Linq;
using UnityEngine;

class GroundFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject groundPrefab;
    [SerializeField]
    private float segmentWidth = 10f;

    private float end = 0f;

    void Start()
    {
        Equation[] equations = new[]
        {
            new Equation(1f, 1f, 0f, 0f)
        };
        ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.Initialise(equations);
        end = segmentWidth;
        MakeAMeshBoii(0f);
    }

    void MakeAMeshBoii(float start)
    {
        GameObject ground = Instantiate(groundPrefab);
        ground.transform.position = Vector3.zero;
        ground.GetComponent<Ground>().Initialise(start, segmentWidth, (int)segmentWidth*10);
    }

    void Update()
    {
        if (Camera.main.transform.position.x >= end - segmentWidth / 2f)
        {
            MakeAMeshBoii(end);
            end += segmentWidth;
        }
    }
}
