using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public int barCurrentHealth;

    public void SetMaxHealth(){

        slider.maxValue = barCurrentHealth;  
        slider.value = barCurrentHealth;
    }

    public void SetHealth(){
        slider.value = barCurrentHealth;
    }
}
