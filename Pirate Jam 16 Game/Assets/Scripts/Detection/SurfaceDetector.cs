using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] protected Collider2D castStart;
    [SerializeField] protected Transform castEnd;

    [Header("")]
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float extraCastDistance = 0f;

    [Header("")]
    [SerializeField] protected bool detectTriggers = false;

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

        Vector2 start = castStart.transform.position;
        Vector2 end = castEnd.position;
        Vector2 direction = (end - start).normalized;

        float contactDistanceMax = Vector2.Distance(start, end);

        var contactFilter = new ContactFilter2D().NoFilter();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
        contactFilter.useTriggers = detectTriggers;

        var hits = new RaycastHit2D[10];
        int hitCount = castStart.Cast(direction, contactFilter, hits, contactDistanceMax + extraCastDistance);
        gotHit = hitCount > 0;
        
        if (gotHit)
        {
            hit = hits[0];

            hitDistance = Vector2.Dot(hit.point - castStart.ClosestPoint(hit.point), direction);
            surfaceDetected = hitDistance <= contactDistanceMax;
        }
        else
        {
            hit = new RaycastHit2D();
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
