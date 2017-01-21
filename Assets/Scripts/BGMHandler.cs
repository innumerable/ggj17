using UnityEngine;
using UnityEngine.Audio;

public class BGMHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D player;

    [SerializeField]
    AudioSource[] sources;
    [SerializeField]
    AudioMixer mainMixer;

    [SerializeField]
    float[] defaultVolumes = { 1, 0, 0 };

    [SerializeField]
    float fadeTime = 1f;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    float minPitch = 0.75f;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    float maxPitch = 1.25f;

    [SerializeField]
    float layer2VelocityLowerThreshold = 30;

    [SerializeField]
    float layer3VelocityLowerThreshold = 60;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = defaultVolumes[i];
            sources[i].loop = true;
        }
	}

    void Update()
    {
        float velocity = player.velocity.x;

        if (velocity > layer2VelocityLowerThreshold)
        {
            sources[1].volume = Mathf.Lerp(sources[1].volume, 1, Time.deltaTime * fadeTime);
        }
        else
        {
            sources[1].volume = Mathf.Lerp(sources[1].volume, 0.2f, Time.deltaTime * fadeTime);
        }

        if (velocity > layer3VelocityLowerThreshold)
        {
            sources[2].volume = Mathf.Lerp(sources[1].volume, 1, Time.deltaTime * fadeTime);
        }
        else
        {
            sources[2].volume = Mathf.Lerp(sources[1].volume, 0.2f, Time.deltaTime * fadeTime);
        }

        mainMixer.SetFloat("MusicPitch", Mathf.Lerp(1f, 1.2f, (velocity - 5f)/100f));
    }
}
