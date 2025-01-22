using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerHealth))]

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] [Range(0, 1)] private float stopDuration = 0.15f;
    private float stopTimer;

    [SerializeField] private float jumpSpeed = 9f;
    [SerializeField] private float jumpableGroundDistance = 0.4f;

    [Header("")]
    [SerializeField] private float hopSpeed = 3.5f;
    [SerializeField] private float hopDelay = 0.2f;
    private float hopTimer;

    [Header("")]
    [SerializeField] private float upGravityScale = 2f;
    [SerializeField] private float downGravityScale = 1.7f;

    [Header("")]
    public AudioClip jumpSound;
    
    [Header("")]
    [SerializeField] private SurfaceDetector GroundDetector;

    private PlayerInput Input;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private PlayerHealth Health;

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Physics = GetComponent<PlayerPhysics>();
        Attack = GetComponent<PlayerAttack>();
        Health = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        HandleMovement();

        IncrementFixedUpdateTimers();
    }

    private void HandleMovement()
    {
        bool onGround = GroundDetector.DetectSurface(jumpableGroundDistance, out bool farHit, out float hitDistance);

        if (farHit && Input.jumpFlag)
        {
            Physics.SetJumpForce(jumpSpeed);
        
            if (jumpSound != null)
                SoundManager.PlaySoundNonSpatial(jumpSound);

            Input.ClearJumpFlag();
        }
        else if (!onGround)
        {
            Physics.SetGroundMoveForce( Input.movementInput * moveSpeed );

            hopTimer = 0f;
            stopTimer = 0f;
        }
        else
        {
            Physics.SetGroundMoveForce( Physics.velocityX * Mathf.Pow(1f - Mathf.Clamp01(stopTimer / stopDuration), 1f) ); // Stop moving

            if (Input.movementInput != 0f && hopTimer > hopDelay)
            {
                Hop();

                hopTimer = 0f;
            }
        }
        
        Physics.AddForce(Physics2D.gravity * Time.fixedDeltaTime * ( Physics.velocityY > 0 ? upGravityScale : downGravityScale ));

        Physics.DoFixedUpdate();
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

        ReadInputs();
    }

    private void ReadInputs()
    {
        if (Input.attackFlag)
        {
            Vector2 direction = Vector2.right * (Input.movementInputActive > 0f ? 1f : -1f);

            Attack.PerformAttack(Attack.FindObjectsToAttack(direction), direction);

            Input.ClearAttackFlag();
            Input.ClearBlockFlag();

            if (Health.blocking)
            {
                Debug.Log("Player stopped blocking");
                Health.blocking = false;
            }
        }
        else
        {
            if (Input.blockFlag)
            {
                if (!Health.blocking)
                    Debug.Log("Player is blocking");

                Health.blocking = true;
            }
            else
            {
                if (Health.blocking)
                    Debug.Log("Player stopped blocking");

                Health.blocking = false;
            }
        }
    }
}
