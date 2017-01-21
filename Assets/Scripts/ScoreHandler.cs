using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    Text scoreTextUI;

	[SerializeField]
    float scorePerUnitDistance = 1000;

    [SerializeField]
    string saveFileSuffix = "/scores.bin";

    long score;

    float originalPositionX;

    List<Tuple<int, long>> hiScores;

    void Start()
    {
        originalPositionX = transform.position.x;
        Debug.Log("Score file location: " + Application.persistentDataPath + saveFileSuffix);
        ReadHiscores();
    }
    
	void Update ()
    {
        float distanceMoved = transform.position.x - originalPositionX;
        score = (int)Mathf.Max(distanceMoved * scorePerUnitDistance, score);
        scoreTextUI.text = string.Format("Score: {0}", score);
	}

    public void AddHiscore()
    {
        hiScores.Add(new Tuple<int, long>(hiScores.Count + 1, score));
        WriteHiscores();
    }

    public void ReadHiscores()
    {
        try
        {
            using (FileStream fs = new FileStream(Application.persistentDataPath + saveFileSuffix, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                try
                {
                    formatter.Serialize(fs, hiScores);
                    hiScores = formatter.Deserialize(fs) as List<Tuple<int, long>>;
                }
                catch (SerializationException e)
                {
                    Debug.Log("Failed to serialize hiscores. Reason: " + e.Message);
                    throw;
                }
            }
        }
        catch (Exception ioex)
        {
            FileStream fs = new FileStream(Application.persistentDataPath + saveFileSuffix, FileMode.Create);
            hiScores = new List<Tuple<int, long>>();
        }
    }

    public void WriteHiscores()
    {
        using (FileStream fs = new FileStream(Application.persistentDataPath + saveFileSuffix, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(fs, hiScores);
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to serialize hiscores. Reason: " + e.Message);
                throw;
            }
        }
    }
}

[Serializable]
public class Tuple<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}