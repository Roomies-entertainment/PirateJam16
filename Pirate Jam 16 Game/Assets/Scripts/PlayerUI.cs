using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Slider HealthBar;

    [SerializeField] private Image fill;

    [SerializeField] private Gradient gradient;

    public void UpdateHealthBar(float health01, DetectionData data = null)
    {
        HealthBar.value = health01;
        fill.color = gradient.Evaluate(HealthBar.normalizedValue);
    }
}
