using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform panel;

    [Header("")]
    [SerializeField] private Vector2 panelPadding = new Vector2(60f, 10f);

    [Header("")]
    public float duration = 4.5f;
    private float displayTimer;

    public float fadeInDuration = 1f;
    private float fadeTimer;

    private bool displaying;

    private void OnValidate()
    {
        if (duration <= 0)
            duration = 0.1f;
    }

    private void Start()
    {
        SetObjectsActive(false);
    }

    private void Update()
    {
        if (!displaying)
        {
            return;
        }

        if (displayTimer > duration)
        {
            CloseHint();

            displaying = false;
        }
        else
        {
            UpdateHint();

            displayTimer += Time.deltaTime;
            fadeTimer += Time.deltaTime;
        }
    }

    public void DisplayHint(string hint)
    {
        if (!enabled)
        {
            return;
        }

        if (PlayerPrefs.GetInt("enableHints") == 0)
        {
            enabled = false;

            return;
        }

        if (hint.Length == 0)
        {
            return;
        }

        //transform.position = Camera.main.WorldToScreenPoint(position);

        text.text = hint;

        displayTimer = 0f;

        if (!displaying)
            fadeTimer = 0f;

        UpdateHint();

        SetObjectsActive(true);
        displaying = true;
    }

    private void CloseHint()
    {
        SetObjectsActive(false);
    }

    private void UpdateHint()
    {
        Vector2 targetSize = text.GetComponent<RectTransform>().sizeDelta + panelPadding;

        panel.sizeDelta = Vector2.Lerp(new Vector2(0f, targetSize.y), targetSize, fadeTimer / fadeInDuration);
        text.color = new Color(text.color.r, text.color.g, text.color.b, ((fadeTimer / fadeInDuration) - 0.85f) / 0.15f);
    }

    private void SetObjectsActive(bool setTo)
    {
        panel.gameObject.SetActive(setTo);
        text.gameObject.SetActive(setTo);
    }
}
