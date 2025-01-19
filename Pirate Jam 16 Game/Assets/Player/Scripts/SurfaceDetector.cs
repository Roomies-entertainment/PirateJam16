using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] private Transform detectionStart;
    [SerializeField] private Transform detectionEnd;

    public bool DetectSurface()
    {
        Vector3 start = detectionStart.position;
        Vector3 end = detectionEnd.position;

        return Physics2D.Raycast(start, (end - start).normalized, Vector2.Distance(start, end));
    }
}
