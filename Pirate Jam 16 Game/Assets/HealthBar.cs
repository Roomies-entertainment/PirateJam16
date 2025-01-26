using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float health01, DetectionData data = null)
    {
        slider.value = health01;
    }
}
