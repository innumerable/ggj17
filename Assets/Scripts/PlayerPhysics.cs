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
    [SerializeField]
    private float jumpVelocity = 10f;

    private float coolDown = 0.5f;
    private float coolDownRemaining;
    private int pressCount = 0;
    private bool canJump = true;
    
    Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coolDownRemaining = coolDown;
    }

	// Update is called once per frame
	void Update ()
	{
        // Decide double tapping
	    bool doubleTap = false;
        if (PlayerInput.IsDown)
        {
            if (coolDownRemaining > 0 && pressCount == 1)
                doubleTap = true;
            else
            {
                coolDownRemaining = coolDown;
                pressCount++;
            }
        }

        // Apply actions
        if (doubleTap && canJump)
		{
		    rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Max(jumpVelocity, jumpVelocity + rb2d.velocity.y));
		    canJump = false;
		}
        else if (PlayerInput.IsPressed)
        {
            rb2d.gravityScale = higherGravity;
        }
        else
        {
            rb2d.gravityScale = defaultGravity;
        }

        // Time tracking
	    if (coolDownRemaining > 0)
	        coolDownRemaining -= Time.deltaTime;
	    else
	        pressCount = 0;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }
}
