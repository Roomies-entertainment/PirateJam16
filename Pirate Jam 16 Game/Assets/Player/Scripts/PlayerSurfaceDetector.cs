using UnityEngine;

public class PlayerSurfaceDetector : SurfaceDetector
{
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
}
