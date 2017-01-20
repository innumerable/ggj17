using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPhysics : MonoBehaviour
{
    [SerializeField]
    float defaultGravity = 1f;
    [SerializeField]
    float higherGravity = 6f;

    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update ()
    {
		if (PlayerInput.IsPressed)
        {
            rb2d.gravityScale = higherGravity;
        }
        else
        {
            rb2d.gravityScale = defaultGravity;
        }
	}
}
