using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Mathematics;
using Unity.VisualScripting;

public class FadingBackground : MonoBehaviour
{
    SpriteRenderer[] backgroundSprites;
    int arraySize;

    public Transform playerObject;

    private float smoothTime = 0.4f;
    private Vector3 velocity = Vector3.zero;

    float playerPositionY;

    public Transform followObject;


    //They said it couldnt be done, And they were right. It shouldnt have been done.

    void Start()
    {
        backgroundSprites = GetComponentsInChildren<SpriteRenderer>();
        arraySize = backgroundSprites.Length;
    }

    //I cannot set it to only process one axis, So have fun fixing this shit

    void Update()
    {
        followObject.position = Vector3.SmoothDamp(followObject.position, playerObject.position, ref velocity, smoothTime);
        playerPositionY = followObject.position.y;

        PressTesting();
    }



    void PressTesting()
    {
        for (int i = 0; i < arraySize; i++)
        {
            backgroundSprites[i].color = new Color(1, 1, 1, (playerPositionY * 0.02f) + 0.8f);
        }
    }
}
