using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippyFX : MonoBehaviour {

    public float intensity;

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
	
    public void StartEffect(float x, float y, float inten)
    {
        if (effectTime > 0f) return;
        intensity = inten;
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
        Graphics.Blit(source, destination, fxMaterial);
    }
}
