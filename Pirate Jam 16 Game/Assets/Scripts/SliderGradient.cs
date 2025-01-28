using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SliderGradient : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private float lastSliderValue = 0f;

    private void OnValidate()
    {
        Start();
    }

    private void Start()
    {
        UpdateColor(lastSliderValue);
    }

    public void UpdateColor(float value)
    {
        fill.color = gradient.Evaluate(value);

        lastSliderValue = value;
    }
}
