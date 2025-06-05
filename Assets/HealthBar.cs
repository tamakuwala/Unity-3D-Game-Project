using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slide;
    public Gradient gradient;
    public Image healthcolor;

    public void SetMaxHealth(float health)
	{
		slide.maxValue = health;
		slide.value = health;
        healthcolor.color = gradient.Evaluate(1f);
	}

    public void SetHealth(float health)
	{
		slide.value = health;
        healthcolor.color = gradient.Evaluate(slide.normalizedValue);
	}
}
