using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider HealthBar;

    public void UpdateHealthBar(float health01)
    {
        HealthBar.value = health01;
    }
}
