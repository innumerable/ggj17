using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour
{
    [SerializeField]
    ParticleSystem poofSystemPrefab;
    [SerializeField]
    float groundHitPoofVelocityThreshold;
    ParticleSystem poofSystem;

    void Start()
    {
        poofSystem = Instantiate(poofSystemPrefab, transform);
        poofSystem.transform.localPosition = Vector3.zero;
        poofSystem.Stop();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.relativeVelocity.magnitude > groundHitPoofVelocityThreshold)
        {
            poofSystem.Play();
        }
    }

    void LateUpdate()
    {
        poofSystem.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
    }
}
