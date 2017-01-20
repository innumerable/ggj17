using System.Linq;
using UnityEngine;

class GroundFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject groundPrefab;
    [SerializeField]
    private float segmentWidth = 10f;


    void Start()
    {
        Equation[] equations = new[]
        {
            new Equation(1f, 1f, 0f, 0f),
            new Equation(0.2f, 10f, 0f, 0f), 
            new Equation(5f, 0.05f, 1f, -1f), 
        };
        ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean.Initialise(equations);
        MakeAMeshBoii();
    }

    void MakeAMeshBoii()
    {
        GameObject ground = Instantiate(groundPrefab);
        ground.transform.position = Vector3.zero;
        ground.GetComponent<Ground>().Initialise(0f, segmentWidth, (int)segmentWidth*10);
    }
}
