using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThreshold : MonoBehaviour
{
    [SerializeField] private CameraFollow CameraFollowScript;

    [Header("")]
    public float negThresholdX = 0.5f;
    public float posThresholdX = 0.5f;
    public float negThresholdY = 0.5f;
    public float posThresholdY = 0.5f;

    private void Update()
    {
        if (CameraFollowScript == null)
        {
            Debug.Log($"{this} in Update() - CameraFollowScript is null - attempting to find on {gameObject.name}");

            CameraFollowScript = GetComponent<CameraFollow>();

            return;
        }
    }
}
