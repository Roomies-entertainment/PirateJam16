using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider HealthBar;

    public void OnTakeDamage(float health01, DetectionData data)
    {
        HealthBar.value = health01;
    }
}
