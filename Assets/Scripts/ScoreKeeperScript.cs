using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeperScript : MonoBehaviour
{
    public static ScoreKeeperScript instance;

    public int level = 0;
    public GameObject scorePopUp;

    private float score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);
    }

    public void CalculateScore()
    {
        score += TreadmillGeneration.instance.GetTimeRemaining();
    }

    public void AddScore(int num)
    {
        var popup = Instantiate(scorePopUp);
        int trueScore = num * Player.instance.GetSpeedBoostCount();
        if (trueScore <= 0)
            trueScore = num;
        // popup.transform.parent = Player.instance.transform;
        popup.GetComponent<TextMeshPro>().text = trueScore.ToString();

        score += trueScore;
        
    }

    public int GetScore()
    {
        return (int)score;
    }

    public void DecreaseLevel()
    {
        level--;
        if (level <= -2)
            level = -2;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
