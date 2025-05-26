using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float jumpSpeed = 9.5f;
    public float stopDuration = 0.15f;
    public float stopTimer { get; private set; }
    
    [Header("")]
    public float hopSpeed = 3.5f;
    public float hopDelay = 0.15f;
    public float hopTimer { get; private set; }

    [Header("")]
    public float upGravityScale = 1.7f;
    public float downGravityScale = 1.7f;

    [Header("")]
    public float fallThroughPlatformSpeed = 5f;

    [Header("")]
    [SerializeField] private UnityEvent onWalkHop;
    [SerializeField] private UnityEvent onJump;

    public float jumpDampTimer { get; private set; }
    public const float JumpDampDuration = 0.1f;

    public void IncrementGroundedTimers()
    {
        hopTimer += Time.fixedDeltaTime;
        stopTimer += Time.fixedDeltaTime;
    }

    public void ResetGroundedTimers()
    {
        hopTimer = 0f;
        stopTimer = 0f;
    }

    public void IncrementJumpTimers()
    {
        jumpDampTimer += Time.deltaTime;
    }

    public void ResetJumpTimers()
    {
        jumpDampTimer = 0f;
    }
/* 
    public Vector3 JumpVector()
    {

    }
 */
    public void OnJump()
    {
        onJump?.Invoke();
    }

    public void OnWalkHop()
    {
        onWalkHop?.Invoke();

        hopTimer = 0f;
    }
}
