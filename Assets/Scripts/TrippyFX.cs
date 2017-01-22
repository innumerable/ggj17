using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippyFX : MonoBehaviour {

    private float intensity;
    private float xi;
    private float yi;
    private float tscale;

    [SerializeField]
    private Shader fxShader;

    private Material fxMaterial;
    private float effectTime = 0f;

    [SerializeField]
    private Texture normal;

    void Awake()
    {
        fxMaterial = new Material(fxShader); 
    }
	
    public void StartEffect(float x, float y, float inten, bool ov = false, float ts = 5f)
    {
        if (effectTime > 0f && !ov) return;
        intensity = inten;
        xi = x;
        yi = y;
        tscale = ts;
        StartCoroutine(Impact());
    }

    IEnumerator Impact()
    {
        effectTime = 0f;
        while ((effectTime += Time.deltaTime) < 0.2f)
            yield return null;
        effectTime = 0f;
    }

	// Update is called once per frame
	void Update () {

	}

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        fxMaterial.SetTexture("_vDisp", normal);
        fxMaterial.SetFloat("_inten", intensity);
        fxMaterial.SetFloat("_time", effectTime);
        fxMaterial.SetFloat("_xImpact", xi);
        fxMaterial.SetFloat("_timescale", tscale);
        fxMaterial.SetFloat("_yImpact", yi);
        Graphics.Blit(source, destination, fxMaterial);
    }
}
