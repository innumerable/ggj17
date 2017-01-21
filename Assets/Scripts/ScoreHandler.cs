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
    float scorePerUnitDistance = 1;

    [SerializeField]
    string saveFileSuffix = "/scores.bin";

    [SerializeField]
    int numScoresToShow = 10;

    long score;

    float originalPositionX;

    List<Tuple<int, long>> hiScores;

    void Start()
    {
        originalPositionX = transform.position.x;
        ReadHiscores();
    }
    
	void Update ()
    {
        float distanceMoved = transform.position.x - originalPositionX;
        score = (int)Mathf.Max(distanceMoved * scorePerUnitDistance, score);
        scoreTextUI.text = string.Format("Score: {0}", score);
	}

    public void ShowHiscores(Text text)
    {
        string str = "";
        for (int i = 0; i < hiScores.Count && i < numScoresToShow; i++)
        {
            str += hiScores[i].First.ToString() + " : " + hiScores[i].Second.ToString() + Environment.NewLine;
        }

        text.text = "HISCORES" + Environment.NewLine + "PLAYER NUMBER : SCORE" + Environment.NewLine + Environment.NewLine + str;
    }

    public void AddHiscore()
    {
        hiScores.Add(new Tuple<int, long>(hiScores.Count + 1, score));
        hiScores.Sort((emp1, emp2) => emp2.Second.CompareTo(emp1.Second));
        hiScores = hiScores.GetRange(0, Math.Min(numScoresToShow, hiScores.Count));
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
                    hiScores = formatter.Deserialize(fs) as List<Tuple<int, long>>;
                }
                catch (SerializationException e)
                {
                    Debug.Log("Failed to serialize hiscores. Reason: " + e.Message);
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Could not read hiscore file. Creating a new one. Exception: " + ex.ToString());
            FileStream fs = new FileStream(Application.persistentDataPath + saveFileSuffix, FileMode.Create);
            fs.Close();
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