using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteFading : MonoBehaviour
{
    SpriteRenderer thisObj;

    bool isEntered = false;

    void Start()
    {
        thisObj = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D()
    {
        StartCoroutine(FadeTo(0.0f, 1.0f));
    }

    void OnTriggerExit2D()
    {
        StartCoroutine(FadeTo(1.0f, 1.0f));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = thisObj.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.fixedDeltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            thisObj.material.color = newColor;
            isEntered = !isEntered;
            yield return null;
        }
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
