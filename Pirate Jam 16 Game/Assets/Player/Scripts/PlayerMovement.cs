using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float jumpSpeed = 9.5f;
    public float slowSpeed = 0.1f;
    
    [Header("")]
    public float hopVerticalSpeed = 3.5f;
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

    private void Start() { } // Ensures component toggle in inspector

    public void IncrementGroundedTimers()
    {
        hopTimer += Time.fixedDeltaTime;
    }

    public void ResetGroundedTimers()
    {
        hopTimer = 0f;
    }

    public void IncrementJumpTimers()
    {
        jumpDampTimer += Time.deltaTime;
    }

    public void ResetJumpTimers()
    {
        jumpDampTimer = 0f;
    }

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
