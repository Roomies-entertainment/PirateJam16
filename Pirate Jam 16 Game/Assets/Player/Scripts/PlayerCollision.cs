using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Transform Colliders;
    [SerializeField] private Collider2D PhysicsCollider;

    [Header("")]
    [SerializeField] private Transform UprightConfiguration;
    [SerializeField] private Transform FlatConfiguration;
    private Transform lastConfig;
    private Transform currentConfig;

    [Space]
    [SerializeField] private float configChangeDuration = 0.2f;
    private float configChangeTimer = 0.1f;
    private bool configChangeFlag = false;

    public float platformPhaseHoldTimer { get; private set; }
    public const float PlatformPhaseHoldDuration = 0.2f;

    private void TryPhaseThroughPlatform(Collider2D platformCollider)
    {
        if (!Physics2D.GetIgnoreCollision(PhysicsCollider, platformCollider))
        {
            Physics2D.IgnoreCollision(PhysicsCollider, platformCollider, true);
            phasedPlatforms.Add(platformCollider);
        }
    }

    private ContactPoint2D phasableContactPoint;
    public bool GetOnPhasablePlatform(out ContactPoint2D contactPoint)
    {
        contactPoint = phasableContactPoint;

        return !ContactPointNull(phasableContactPoint);
    }

    public enum PlatformPhaseState
    {
        None,
        ForcePhasing,
        Phasing
    }

    public PlatformPhaseState platformPhaseState { get; private set; }
    private List<Collider2D> phasedPlatforms = new();


    [Header("")]
    [SerializeField] [Range(0, 1)] [Tooltip("0 = Nothings a wall | 1 = Everythings a wall\nGets ignored when touching PlatformEffector components")]
    private float wallSensitivity = 0.3f;
    private ContactPoint2D wallContactPoint;
    //public ContactPoint2D GetWallContact() { return wallContactPoint; }
    public bool GetOnWall(out ContactPoint2D contactPoint)
    {
        contactPoint = wallContactPoint;

        return !ContactPointNull(wallContactPoint);
    }

    private bool ContactPointNull(ContactPoint2D contactPoint) { return contactPoint.normal.sqrMagnitude == 0; }

    [Header("")]
    [SerializeField] private bool debug = false;


    public void DoOnCollisionStay2D(Collision2D collision)
    {
        SetContactFlagsAndData(collision, out bool phasingThroughCollider);

        if (platformPhaseState > PlatformPhaseState.None && phasingThroughCollider)
        {
            TryPhaseThroughPlatform(collision.otherCollider);
        }
    }

    public void DoOnCollisionExit2D(Collision2D collision)
    {
        SetContactFlagsAndData(collision, out bool phasingThroughCollider);
    }

    private void SetContactFlagsAndData(Collision2D collision, out bool phasingThroughCollider)
    {
        bool exiting = collision.contactCount == 0;

        float maxNormalYAbs = wallSensitivity;

        // Basically if contacts are null player will know theyre not touching walls etc.

        ContactPoint2D firstContactPoint = exiting ? new ContactPoint2D() : collision.GetContact(0);

        phasableContactPoint = firstContactPoint;
        wallContactPoint = firstContactPoint;

        phasingThroughCollider = true;
  
        if (!exiting)
        {
            var platformEffector = firstContactPoint.collider.GetComponent<PlatformEffector2D>();

            if (platformEffector == null)
            {
                phasableContactPoint = new ContactPoint2D();

                phasingThroughCollider = false;
            }

            if (firstContactPoint.normal.y <= -maxNormalYAbs ||
                firstContactPoint.normal.y >= maxNormalYAbs || (
                    firstContactPoint.collider.usedByEffector &&
                    firstContactPoint.normal.y < Mathf.Sin(platformEffector.sideArc * 0.5f * Mathf.Deg2Rad))
                )
            {
                wallContactPoint = new ContactPoint2D();
            }
        }
    }

    public void StartPhasingThroughPlatforms(Collider2D platformCollider, float minDuration)
    {
        if (platformPhaseState > PlatformPhaseState.None)
        {
            return;
        }

        StartCoroutine(StartPhasingThroughPlatformsCR(platformCollider, minDuration));
    }

    public IEnumerator StartPhasingThroughPlatformsCR(Collider2D platformCollider, float minDuration)
    {
        TryPhaseThroughPlatform(platformCollider);
        
        platformPhaseState = PlatformPhaseState.ForcePhasing;

        float timer = 0f;

        while (timer < minDuration)
        {
            yield return null;

            timer += Time.deltaTime;
        }

        platformPhaseState = PlatformPhaseState.Phasing;
    }

    public void StopPhasingThroughPlatforms()
    {
        if (platformPhaseState == PlatformPhaseState.ForcePhasing)
        {
            return;
        }

        foreach(Collider2D p in phasedPlatforms)
        {
            Physics2D.IgnoreCollision(PhysicsCollider, p, false);
        }

        phasedPlatforms.Clear();

        platformPhaseState = PlatformPhaseState.None;
    }

    public void IncrementTimers(float increment)
    {
        increment = Mathf.Abs(increment);

        platformPhaseHoldTimer += increment;
    }

    public void ResetPhaseTimers()
    {
        platformPhaseHoldTimer = 0.0f;
    }

    private void OnDisable()
    {
        ResetPhaseTimers();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
