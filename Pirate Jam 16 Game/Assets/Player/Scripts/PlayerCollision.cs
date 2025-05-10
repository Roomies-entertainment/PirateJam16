using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Collider2D PhysicsCollider;
    public SurfaceDetector GroundDetector;

    [Header("")]
    [SerializeField] private bool debug = false;

    public float platformPhaseTimer { get; private set; }
    public const float PlatformPhaseHoldDuration = 0.2f;

    public bool onPhasablePlatform { get; private set; }
    public bool phasing { get; private set; }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!onPhasablePlatform && collision.collider.GetComponent<PlatformEffector2D>())
            onPhasablePlatform = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (onPhasablePlatform && collision.collider.GetComponent<PlatformEffector2D>())
            onPhasablePlatform = false;
    }

    public IEnumerator PhaseThroughPlatforms(Collider2D platformCollider, float duration)
    {
        phasing = true;
        Physics2D.IgnoreCollision(PhysicsCollider, platformCollider, true);

        float timer = 0f;

        while (timer < duration && phasing)
        {
            yield return null;

            timer += Time.deltaTime;
        }

        Physics2D.IgnoreCollision(PhysicsCollider, platformCollider, false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void IncrementPhaseTimers()
    {
        platformPhaseTimer += Time.deltaTime;
    }

    public void ResetPhaseTimers()
    {
        platformPhaseTimer = 0.0f;
    }
}
