using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public void StartShake(float duration, float intensity)
    {
        StartCoroutine(Shake(duration, intensity));
        Debug.Log("Hit");
    }

    private IEnumerator Shake(float duration, float intensity)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= intensity * damper;
            y *= intensity * damper;

            Camera.main.transform.localPosition = new Vector3(x, y, -10f);

            yield return null;
        }


    }
}
