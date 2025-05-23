using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleSettings : MonoBehaviour
{
    [SerializeField] private Toggle hintsToggle;

    private bool listening = false;

    private void OnEnable()
    {
        UpdateToggles();

        listening = true;
    }

    private void UpdateToggles()
    {
        hintsToggle.isOn = IntToBool(PlayerPrefs.GetInt("enableHints"));
    }

    public void OnHintsToggle(bool setTo)
    {
        if (listening)
        {
            PlayerPrefs.SetInt("enableHints", BoolToInt(setTo));
        }
    }

    private bool IntToBool(int value)
    {
        return value >= 1;
    }

    private int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }
}
