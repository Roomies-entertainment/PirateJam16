using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Slider HealthBar;

    public void UpdateHealthBar(float health01, DetectionData data = null)
    {
        HealthBar.value = health01;
    }
}
