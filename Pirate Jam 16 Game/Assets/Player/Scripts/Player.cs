using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerPhysics))]

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] [Range(0, 1)] private float stopDuration = 0.15f;
    private float stopTimer;

    [SerializeField] private float jumpSpeed = 9f;

    [Header("")]
    [SerializeField] private float hopSpeed = 3.5f;
    [SerializeField] private float hopDelay = 0.2f;
    private float hopTimer;

    [Header("")]
    [SerializeField] private float upGravityScale = 2f;
    [SerializeField] private float downGravityScale = 1.7f;

    [Header("")]
    [SerializeField] private SurfaceDetector GroundDetector;

    private PlayerInput Input;
    private PlayerCollision Collision;
    private PlayerPhysics Physics;

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Collision = GetComponent<PlayerCollision>();
        Physics = GetComponent<PlayerPhysics>();
    }

    private void FixedUpdate()
    {
        bool onGround = GroundDetector.DetectSurface();

        if (onGround)
        {
            Physics.SetGroundMoveForce( Physics.velocityX * Mathf.Pow(1f - Mathf.Clamp01(stopTimer / stopDuration), 1f) );

            if (Input.jumpFlag)
            {
                Physics.SetJumpForce(jumpSpeed);

                Input.ClearJumpFlag();
            }
            else if (Input.movementInput != 0f && hopTimer > hopDelay)
            {
                Hop();

                hopTimer = 0f;
            }
        }
        else
        {
            Physics.SetGroundMoveForce( Input.movementInput * moveSpeed);

            hopTimer = 0f;
            stopTimer = 0f;
        }
        
        Physics.AddForce(Physics2D.gravity * Time.fixedDeltaTime * ( Physics.velocityY > 0 ? upGravityScale : downGravityScale ));

        Physics.DoFixedUpdate();

        IncrementFixedUpdateTimers();
    }

    private void Hop()
    {
        Physics.SetJumpForce(hopSpeed);
    }

    private void IncrementFixedUpdateTimers()
    {
        hopTimer += Time.fixedDeltaTime;
        stopTimer += Time.fixedDeltaTime;
    }

    private void Update()
    {
        Input.DoUpdate();
    }
}
