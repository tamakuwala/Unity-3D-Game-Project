using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text scoreText;

    private void Update()
    {
        if (GameManager.instance != null)
            scoreText.text = $"Score: {ScoreKeeperScript.instance.GetScore()}";
    }
}
