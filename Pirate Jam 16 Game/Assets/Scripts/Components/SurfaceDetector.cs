using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] private Transform detectionStart;
    [SerializeField] private Transform detectionEnd;

    [Header("")]
    [SerializeField] private Transform StartUprightConfiguration;
    [SerializeField] private Transform EndUprightConfiguration;
    [SerializeField] private Transform StartFlatConfiguration;
    [SerializeField] private Transform EndFlatConfiguration;

    private void Start()
    {
        UpdateConfiguration(StartUprightConfiguration, EndUprightConfiguration);
    }

    public void OnPerformAttack()
    {
        UpdateConfiguration(StartFlatConfiguration, EndFlatConfiguration);
    }

    public void OnStopAttack()
    {
        UpdateConfiguration(StartUprightConfiguration, EndUprightConfiguration);
    }

    private void UpdateConfiguration(Transform startConfiguration, Transform endConfiguration)
    {
        detectionStart.position = startConfiguration.position;
        detectionEnd.position = endConfiguration.position;
    }

    public bool DetectSurface(float extraHitDistance, out bool gotHit, out float hitDistance, LayerMask layerMask)
    {
        Vector2 start = detectionStart.position;
        Vector2 end = detectionEnd.position;
        Vector2 direction = (end - start).normalized;

        float contactDistanceMax = Vector2.Distance(start, end);
        hitDistance = 0f;

        gotHit = false;

        RaycastHit2D hit = Physics2D.Raycast(start, direction, contactDistanceMax + extraHitDistance, layerMask);

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
