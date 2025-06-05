using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentSpeed : MonoBehaviour
{

   public TMP_Text scoreText;

    private void Update()
    {
        scoreText.text = $"Current Speed: {(int)TreadmillGeneration.instance.GetTreadmillSpeed()}";
    }
}