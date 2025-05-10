using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeedX = 1f;
    [SerializeField] private float followSpeedY = 1f;

    [Header("")]
    public float minOffsetY = 1.5f;
    public float maxOffsetY = 3f;

    [Header("")]
    [SerializeField] public Transform TargetObject;
    [SerializeField] private Camera Camera;

    public Vector3 tWorldPosClamped { get; private set; }
    public Vector2 tScreenPosN1P1Clamped { get; private set; }

    public bool followX { get; private set; } = true;
    public bool followY { get; private set; } = true;

    private void LateUpdate()
    {
        if (Camera == null)
        {
            Debug.Log($"{this} in FollowTarget() - cam is null - attempting to find camera on {gameObject.name}");

            Camera = GetComponent<Camera>();

            return;
        }

        if (TargetObject == null)
        {
            //Debug.Log($"{this} in FollowTarget() - TargetObject is null");

            return;
        }

        float minPosY = TargetObject.position.y + minOffsetY;
        float maxPosY = TargetObject.position.y + maxOffsetY;

        tWorldPosClamped = new Vector2(TargetObject.position.x, Mathf.Clamp(TargetObject.position.y, minPosY, maxPosY));
        tScreenPosN1P1Clamped = ScreenPosToN1P1(Camera.WorldToScreenPoint(tWorldPosClamped));

        FollowTarget(); 
    }

    private void FollowTarget()
    {
        Vector2 tPos = tScreenPosN1P1Clamped;

        if (followX)
            transform.Translate( Vector2.right * (tPos.x * followSpeedX * Time.deltaTime * 16f) );

        if (followY)
            transform.Translate( Vector2.up * (tPos.y * followSpeedY * Time.deltaTime * 16f) );
    }

    private Vector2 ScreenPosToN1P1(Vector2 pixelCoords)
    {
        pixelCoords.x = PixelCoordToN1P1(pixelCoords.x, 0);
        pixelCoords.y = PixelCoordToN1P1(pixelCoords.y, 1);

        return pixelCoords;
    }

    private float PixelCoordToN1P1(float pixelCoord, int xOrY)
    {
        pixelCoord /= xOrY == 0 ? Screen.width : Screen.height;
        pixelCoord -= 0.5f;
        pixelCoord *= 2.0f;

        return pixelCoord;
    }
}
