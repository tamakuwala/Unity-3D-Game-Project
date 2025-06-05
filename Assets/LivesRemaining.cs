using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesRemaining : MonoBehaviour
{
    public TMP_Text liveText;

    private void Update()
    {
    
        liveText.text = $"Lives: {GameManager.instance.lives}";
    }
}