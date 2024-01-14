using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void setMaxHealth(float health) //hp bar 게이지
    {
        slider.maxValue = health; //hp 최댓값.
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void setHealth(float health) //초기 설정 hp bar
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}


