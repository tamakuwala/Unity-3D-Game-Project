using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    public Slider speedBar;
    public Gradient colorGradient;
    public Image speed;

    public void setMaxSpeed(float value){
        speedBar.minValue = value;
        speedBar.value = value;
        speed.color = colorGradient.Evaluate(1f);
    }

    public void SetSpeed(float value){
        speedBar.value = value;
        speed.color = colorGradient.Evaluate(speedBar.normalizedValue);
    }
}
