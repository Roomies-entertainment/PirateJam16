using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{

    bool isMoving = true;

    public float speed;

    public float scrollStopDelay = 110;
 
    // Update is called once per frame
    void Update()
    {
        if (isMoving == true){
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    private void Awake()
    {
        StartCoroutine(StopScrolling());
    }

    IEnumerator StopScrolling()
    {
        yield return new WaitForSecondsRealtime(scrollStopDelay);

        isMoving = false;
    }
}
