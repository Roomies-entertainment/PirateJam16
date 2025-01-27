using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SliderGradient : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    private void Update()
    {
        if (slider == null || fill == null)
            return;
        
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
