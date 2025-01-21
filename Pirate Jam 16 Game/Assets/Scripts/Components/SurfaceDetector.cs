using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] private Transform detectionStart;
    [SerializeField] private Transform detectionEnd;

    public bool DetectSurface(float extraHitDistance, out bool gotHit, out float hitDistance)
    {
        Vector2 start = detectionStart.position;
        Vector2 end = detectionEnd.position;
        Vector2 direction = (end - start).normalized;

        float contactDistanceMax = Vector2.Distance(start, end);
        hitDistance = 0f;

        gotHit = false;

        RaycastHit2D hit = Physics2D.Raycast(start, direction, contactDistanceMax + extraHitDistance);

        if (hit.transform != null)
        {
            gotHit = true;

            hitDistance = Vector2.Dot(hit.point - start, direction);

            if (hitDistance <= contactDistanceMax)
                return true;
        }

        return false;
    }
}
