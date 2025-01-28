using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("")]
    [SerializeField] private float hopSpeed = 3.5f;
    [SerializeField] private float hopDelay = 0.2f;
    private float hopTimer;

    [Header("")]
    [SerializeField] private float upGravityScale = 2f;
    [SerializeField] private float downGravityScale = 1.7f;
    
    [Header("")]
    [SerializeField] private UnityEvent onWalkHop;
    [SerializeField] private UnityEvent onJump;

    [Header("")]
    [SerializeField] private SurfaceDetector GroundDetector;

    private PlayerInput Input;
    private PlayerCollision Collision;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private PlayerAnimation Animation;
    private PlayerHealth Health;

    private float platformPhaseTimer;
    private const float PlatformPhaseHoldDuration = 0.2f;

    private float jumpDampTimer;
    private const float JumpDampDuration = 0.1f;

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Collision = GetComponent<PlayerCollision>();
        Physics = GetComponent<PlayerPhysics>();
        Attack = GetComponent<PlayerAttack>();
        Animation = GetComponent<PlayerAnimation>();
        Health = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        HandleMovement();

        IncrementFixedUpdateTimers();
    }

    private void HandleMovement()
    {
        bool farHit = GroundDetector.gotHit;
        bool onGround = GroundDetector.surfaceDetected;

        if ((onGround || farHit && Physics.velocityY > 0f) && Input.jumpFlag)
        {
            Physics.SetJumpForce(jumpDampTimer < JumpDampDuration ? jumpSpeed * 0.7f : jumpSpeed);
        
            onJump.Invoke();

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

        Animation.DoUpdate(Input.movementInput);
    }

    private void Hop()
    {
        onWalkHop.Invoke();
        
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

    private void LateUpdate()
    {
        ReadInputs();

        platformPhaseTimer += Time.deltaTime;
        jumpDampTimer += Time.deltaTime;
    }

    private void ReadInputs()
    {
        if (Input.verticalInput >= 0f)
            platformPhaseTimer = 0.0f;

        if (Collision.onPhasablePlatform && !Collision.phasing && Input.verticalInput < -0.35f && platformPhaseTimer >= PlatformPhaseHoldDuration)
        {
            Collision.StartCoroutine(Collision.PhaseThroughPlatforms(0.1f));
            Physics.SetForce(new Vector2(Physics.velocityX, -10f));
        }
        else if (Input.attackFlag)
        {
            if (Attack.attackTimer > Attack.AttackDuration)
            {
                Vector2 direction = Vector2.right * (Input.movementInputActive > 0f ? 1f : -1f);

                List<ComponentData> enemies = Attack.FindObjectsToAttack(direction);
                Attack.AttackObjects(enemies, transform.position, direction, CalculateAttackDamage());
            }

            Input.ClearBlockFlag();

            if (Health.blocking)
            {
                Health.StopBlocking();
            }

            jumpDampTimer = 0f;
        }
        else if (Attack.attackTimer > Attack.AttackDuration)
        {
            if (Attack.attacking)
            {
                Attack.StopAttack();
            }

            if (Input.blockFlag)
            {
                Health.StartBlocking();
            }
            else if (Health.blocking)
            {
                Health.StopBlocking();
            }
        }
    }

    private int CalculateAttackDamage()
    {
        return Physics.velocityY < Physics2D.gravity.y * downGravityScale * Attack.fallingThreshold ?
        PlayerAttack.BaseDamage + Attack.fallingExtraDamage :
        PlayerAttack.BaseDamage;
    }
}
