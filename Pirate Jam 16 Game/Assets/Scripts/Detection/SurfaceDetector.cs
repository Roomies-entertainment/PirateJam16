using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] private Detection.CastType2D castShape;

    [Header("")]
    [SerializeField] private float castShapeWidth;
    [SerializeField] private float castShapeLength;
    [SerializeField] private float castShapeAngle;
    [SerializeField] private CapsuleDirection2D capsuleDirection;

    [Header("")]
    [SerializeField] protected Transform detectionStart;
    [SerializeField] protected Transform detectionEnd;

    [Header("")]
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float extraCastDistance = 0f;

    [Header("")]
    [SerializeField] protected UnityEvent onSurfaceEnter;
    [SerializeField] protected UnityEvent onSurfaceStay;
    [SerializeField] protected UnityEvent onSurfaceExit;

    public bool gotHit { get; private set; }
    public bool surfaceDetected { get; private set; }
    public RaycastHit2D hit { get; private set; }
    public float hitDistance { get; private set; }

    private void Update()
    {
        DetectSurface();
    }

    public void DetectSurface()
    {
        bool surfaceDetectedStore = surfaceDetected;

        Vector2 start = detectionStart.position;
        Vector2 end = detectionEnd.position;
        Vector2 direction = (end - start).normalized;

        float contactDistanceMax = Vector2.Distance(start, end);

        switch (castShape)
        {
            case Detection.CastType2D.Ray:
                hit = Physics2D.Raycast(start, direction, contactDistanceMax + extraCastDistance, layerMask);
                break;

            case Detection.CastType2D.Circle:
                hit = Physics2D.CircleCast(start, castShapeWidth * 0.5f, direction, contactDistanceMax + extraCastDistance, layerMask);
                break;

            case Detection.CastType2D.Box:
                hit = Physics2D.BoxCast(start, new Vector2(castShapeWidth, castShapeLength), castShapeAngle, direction, contactDistanceMax + extraCastDistance, layerMask);
                break;

            case Detection.CastType2D.Capsule:
                hit = Physics2D.CapsuleCast(
                    start, new Vector2(castShapeWidth, castShapeLength), capsuleDirection, castShapeAngle, direction, contactDistanceMax + extraCastDistance, layerMask);
                break;
        }

        gotHit = hit.transform != null;

        if (gotHit)
        {
            hitDistance = Vector2.Dot(hit.point - start, direction);
            surfaceDetected = hitDistance <= contactDistanceMax;
        }
        else
        {
            hitDistance = 0f;
            surfaceDetected = false;
        }

        if (!surfaceDetectedStore && surfaceDetected)
        {
            onSurfaceEnter?.Invoke();
        }
        else if (surfaceDetectedStore && surfaceDetected)
        {
            onSurfaceStay?.Invoke();
        }
        else if (surfaceDetectedStore && !surfaceDetected)
        {
            onSurfaceExit?.Invoke();
        }
    }
}
