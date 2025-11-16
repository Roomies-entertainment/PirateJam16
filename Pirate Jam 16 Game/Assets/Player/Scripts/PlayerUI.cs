using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Slider HealthBar;

    public TextMeshProUGUI counterText;

    [HideInInspector] public int deathCounter;

    [SerializeField] private GameObject deathScreen;

    public void UpdateHealthBar(float health01, DetectionData data = null)
    {
        HealthBar.value = health01;
    }

    public void IncreaseDeathCounter()
    {
        deathCounter++;
        counterText.text = deathCounter.ToString();
    } 

    public void SetDeathScreenActive(bool setTo)
    {
        deathScreen.SetActive(setTo);
    }
}
