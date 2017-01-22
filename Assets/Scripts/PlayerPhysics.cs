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
    [SerializeField]
    private ParticleSystem jumpParticleSystem;
    [SerializeField]
    private ParticleSystem canJumpParticleSystem;
    [SerializeField]
    private ParticleSystem bounceParticleSystem;
    [SerializeField]
    private float bounceVelocityThreshold = 15f;
    [Header("Boost Effects")]
    [SerializeField]
    private float backwardsVelocity = 15f;
    [SerializeField]
    private float bounceVelocity = 15f;
    [SerializeField]
    private PhysicsMaterial2D bouncyMaterial;
    [SerializeField]
    private PhysicsMaterial2D normalMaterial;
    [SerializeField]
    private float bouncyDuration = 5f;
    [SerializeField]
    private float glideVelocity = 15f;
    [SerializeField]
    private float glideGravityMultiplier = 0.1f;
    [SerializeField]
    private float glideDuration = 2f;
    [SerializeField]
    private float jumpBoostVelocity = 15f;

    //
    [Header("ShaderReference")]
    [SerializeField]
    private TrippyFX impactEffect;

    private float coolDown = 0.5f;
    private float coolDownRemaining;
    private int pressCount = 0;
    private bool canJump = true;
    private float currentGravityMultiplier = 1f;

    public bool CanJump
    {
        get { return canJump; }
        set
        {
            canJump = value;
            if (value)
                canJumpParticleSystem.Play();
            else
                canJumpParticleSystem.Pause();
        }
    }

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
        if (doubleTap && CanJump)
		{

		    rb2d.velocity = new Vector2(rb2d.velocity.x + jumpVelocity/10f, Mathf.Max(jumpVelocity, jumpVelocity + rb2d.velocity.y));

		    jumpParticleSystem.transform.rotation = Quaternion.AngleAxis(Mathf.Atan(10f)-Mathf.PI, Vector3.left);
            jumpParticleSystem.Play();
		    CanJump = false;
		}
        else if (PlayerInput.IsPressed)
        {
            rb2d.gravityScale = higherGravity * currentGravityMultiplier;
        }
        else
        {
            rb2d.gravityScale = defaultGravity * currentGravityMultiplier;
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
            CanJump = true;
            //if (collision.relativeVelocity.magnitude > 25f)
            //{
            //    //var wtss = Camera.main.WorldToViewportPoint(collision.transform.position);
            //    impactEffect.StartEffect(
            //        .5f,
            //        .5f,
            //        Mathf.LerpUnclamped(
            //            0.01f,
            //            0.5f,
            //            collision.relativeVelocity.magnitude / 1000f)
            //        );
            //    //Mathf.Pow(collision.relativeVelocity.magnitude / 500f, 2));


            //}
        }

        if (collision.relativeVelocity.magnitude > bounceVelocityThreshold)
        {
            bounceParticleSystem.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Backwards":
                BackwardsBoost();
                break;
            case "Bounce":
                BounceBoost();
                break;
            case "Glide":
                GlideBoost();
                break;
            case "Jump":
                JumpBoost();
                break;
            default:
                break;
        }
        Destroy(other.gameObject);
    }

    void ApplyBoost()
    {
        CanJump = true;
    }

    void BackwardsBoost()
    {
        ApplyBoost();
        rb2d.velocity = rb2d.velocity + backwardsVelocity * Vector2.left;
    }

    void BounceBoost()
    {
        ApplyBoost();
        rb2d.velocity = rb2d.velocity + bounceVelocity * Vector2.down;
        StartCoroutine(BeBouncy());
    }

    IEnumerator BeBouncy()
    {
        rb2d.sharedMaterial = bouncyMaterial;
        yield return new WaitForSeconds(bouncyDuration);
        rb2d.sharedMaterial = normalMaterial;
    }

    void GlideBoost()
    {
        ApplyBoost();
        rb2d.velocity = rb2d.velocity + glideVelocity * Vector2.right;
        StartCoroutine(GlideGravity());
    }
    
    IEnumerator GlideGravity()
    {
        float time = glideDuration;
        while ((time -= Time.deltaTime) > 0f)
        {
            currentGravityMultiplier = Mathf.Lerp(1f, glideGravityMultiplier, time/glideDuration);
            yield return null;
        }
        currentGravityMultiplier = 1f;
    }

    void JumpBoost()
    {
        ApplyBoost();
        rb2d.velocity = rb2d.velocity + jumpBoostVelocity*Vector2.up;
    }
}
