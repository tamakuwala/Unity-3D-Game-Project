using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeRemainingScript : MonoBehaviour
{
    public TMP_Text scoreText;

    private void Update()
    {
        if(TreadmillGeneration.instance.GetTimeRemaining() < 10){
            scoreText.color = Color.red ;
        }
        if(TreadmillGeneration.instance.GetTimeRemaining() > 10){
            scoreText.color = Color.white ;
        }
        scoreText.text = $"Time Remaining: {(int)TreadmillGeneration.instance.GetTimeRemaining()}";
    }
}
