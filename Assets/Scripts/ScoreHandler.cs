using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    Text scoreTextUI;

	[SerializeField]
    float scorePerUnitDistance = 1000;

    long score;

    float originalPositionX;

    void Start()
    {
        originalPositionX = transform.position.x;
    }
    
	void Update ()
    {
        float distanceMoved = transform.position.x - originalPositionX;
        score = (int)Mathf.Max(distanceMoved * scorePerUnitDistance, score);
        scoreTextUI.text = string.Format("Score: {0}", score);
        
	}
}
