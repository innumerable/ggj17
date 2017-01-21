using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffects : MonoBehaviour
{
    [SerializeField]
    private float thresholdVelocity;

    private ScreenShake screenShake;
    
	void Start()
	{
	    screenShake = Camera.main.gameObject.GetComponent<ScreenShake>();
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
	    float vel = collision.relativeVelocity.magnitude;
        if (vel > thresholdVelocity)
        {
		    screenShake.StartShake(0.1f, vel/30f);
        }
    }
}
