using UnityEngine;

public class PlayerSurfaceDetector : SurfaceDetector
{
    [Header("")]
    [SerializeField] private Collider2D StartUpright;
    [SerializeField] private Transform EndUpright;
    
    [Header("")]
    [SerializeField] private Collider2D StartFlat;
    [SerializeField] private Transform EndFlat;

    private void Start()
    {
        UpdateConfiguration(StartUpright, EndUpright);
    }

    public void SetFlatConfiguration()
    {
        UpdateConfiguration(StartFlat, EndFlat);
    }

    public void SetUprightConfiguration()
    {
        UpdateConfiguration(StartUpright, EndUpright);
    }

    private void UpdateConfiguration(Collider2D start, Transform end)
    {
        castStart = start;
        castEnd = end;
    }
}
