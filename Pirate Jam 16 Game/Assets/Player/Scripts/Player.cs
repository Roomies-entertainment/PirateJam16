using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(Health))]

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
    [SerializeField] private SurfaceDetector GroundDetector;

    [Header("")]
    public bool takeOneDamage;

    private PlayerInput Input;
    private PlayerCollision Collision;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private Health Health;

    private bool blocking;

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Collision = GetComponent<PlayerCollision>();
        Physics = GetComponent<PlayerPhysics>();
        Attack = GetComponent<PlayerAttack>();
        Health = GetComponent<Health>();
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

        if (takeOneDamage)
        {
            TakeDamage(1);

            Debug.Log($"Player health has reached {Health.health}");

            takeOneDamage = false;
        }
    }

    private void ReadInputs()
    {
        if (Input.attackFlag)
        {
            Attack.AttackEnemies( Vector2.right * ( Input.movementInputActive > 0f ? 1f : -1f ) );

            Input.ClearAttackFlag();

            blocking = false;
        }
        else
        {
            if (Input.blockFlag)
            {
                if (!blocking)
                    Debug.Log("Player is blocking");

                blocking = true;
            }
            else
            {
                if (blocking)
                    Debug.Log("Player stopped blocking");

                blocking = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (blocking)
        {
            Debug.Log("Damage blocked");

            return;
        }
        else
            Health.IncrementHealth(-damage);
    }
}
