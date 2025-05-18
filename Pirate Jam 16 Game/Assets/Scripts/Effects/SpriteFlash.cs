using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.red;

    private SpriteRenderer rend;
    private Color baseColor;

    [SerializeField] private float flashDuration = 1f;
    private float timer = 0f;
    private bool flashing;

    void Start()
    {
        rend = gameObject.GetComponentInParent<SpriteRenderer>();
        baseColor = rend.color;
    }

    public void Flash()
    {
        flashing = true;
        timer = 0f;
    }

    private void Update()
    {
        if (flashing)
        {
            rend.color = Color.Lerp(flashColor, baseColor, timer / flashDuration);

            if (timer >= flashDuration)
            {
                flashing = false;
            }

            timer += Time.deltaTime;
        }
    }
}
