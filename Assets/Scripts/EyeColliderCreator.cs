using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class EyeColliderCreator : MonoBehaviour
{
	void Start()
	{
	    int points = 32;

        Vector2[] edgePoints = new Vector2[points + 1];

	    for (int i = 0; i <= points; i++)
	    {
	        float angle = (Mathf.PI*2.0f/points)*i;
            edgePoints[i] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * transform.localScale.x / 2f;
	    }

        GetComponent<EdgeCollider2D>().points = edgePoints;
	}
}
