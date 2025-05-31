using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Slider HealthBar;

    public TextMeshProUGUI counterText;

    private static int deathCounter;

    public void UpdateHealthBar(float health01, DetectionData data = null)
    {
        HealthBar.value = health01;
    }


    private void Awake()
    {
        counterText.text = deathCounter.ToString();
    }

    private void Start() { } // Ensures component toggle in inspector

    public void DeathCounter(){
        deathCounter++;
        counterText.text = deathCounter.ToString();
    } 
}
