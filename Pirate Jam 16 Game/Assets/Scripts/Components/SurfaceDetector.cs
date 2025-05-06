using UnityEngine;
using UnityEngine.Events;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] protected Transform detectionStart;
    [SerializeField] protected Transform detectionEnd;

    [Header("")]
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float extraCastDistance = 0f;

    [Header("")]
    [SerializeField] protected UnityEvent onHitEnter;
    [SerializeField] protected UnityEvent onHitExit;

    [Header("")]
    [SerializeField] protected UnityEvent onSurfaceEnter;
    [SerializeField] protected UnityEvent onSurfaceExit;

    public bool gotHit { get; private set; }
    public bool surfaceDetected { get; private set; }

    public float hitDistance { get; private set; }

    private void Update()
    {
        DetectSurface();
    }

    public void DetectSurface()
    {
        bool gotHitStore = gotHit;
        bool surfaceDetectedStore = surfaceDetected;

        Vector2 start = detectionStart.position;
        Vector2 end = detectionEnd.position;
        Vector2 direction = (end - start).normalized;

        float contactDistanceMax = Vector2.Distance(start, end);

        RaycastHit2D hit = Physics2D.Raycast(start, direction, contactDistanceMax + extraCastDistance, layerMask);

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

        if (!gotHitStore && gotHit)
        {
            onSurfaceEnter?.Invoke();
        }
        else if (gotHitStore && !gotHit)
        {
            onSurfaceExit?.Invoke();
        }

        if (!surfaceDetectedStore && surfaceDetected)
        {
            onSurfaceEnter?.Invoke();
        }
        else if (surfaceDetectedStore && !surfaceDetected)
        {
            onSurfaceExit?.Invoke();
        }
    }
}
