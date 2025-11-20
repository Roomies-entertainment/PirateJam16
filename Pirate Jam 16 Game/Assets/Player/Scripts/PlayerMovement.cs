using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float aerialSlowSpeed = 0.0f;

    [Header("")]
    public float jumpSpeed = 7.5f;
    public float jumpHoldDuration = 0.2f;   public float jumpHoldTimer { get; private set; }

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

    [HideInInspector] public float speedX, speedY;
    public void SetSpeeds(float speedX, float speedY)
    {
        this.speedX = speedX;
        this.speedY = speedY;
    }

    public void GetSpeeds(out float speedX, out float speedY)
    {
        speedX = this.speedX;
        speedY = this.speedY;
    }

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
        jumpHoldTimer += Time.deltaTime;
    }

    public void ResetJumpTimers()
    {
        jumpHoldTimer = 0f;
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

    private void OnDisable()
    {
        ResetGroundedTimers();
        ResetJumpTimers();
    }
}