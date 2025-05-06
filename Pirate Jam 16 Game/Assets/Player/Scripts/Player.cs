using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerHealth))]

public class Player : MonoBehaviour
{
    [SerializeField] private bool enableBlocking;

    private PlayerInputs Inputs;
    private PlayerCollision Collision;
    private PlayerMovement Movement;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private PlayerAnimation Animation;
    private PlayerHealth Health;

    private void Awake()
    {
        Inputs = GetComponent<PlayerInputs>();
        Collision = GetComponent<PlayerCollision>();
        Movement = GetComponent<PlayerMovement>();
        Physics = GetComponent<PlayerPhysics>();
        Attack = GetComponent<PlayerAttack>();
        Animation = GetComponent<PlayerAnimation>();
        Health = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        Physics.InitializeRigidbody();
    }

    private void FixedUpdate()
    {
        HandleMovement();

        Movement.IncrementGroundedTimers();
    }

    private void HandleMovement()
    {
        bool farHit = Collision.GroundDetector.gotHit;
        bool onGround = Collision.GroundDetector.surfaceDetected;

        if ((onGround || farHit && Physics.velocityY > 0f) && Inputs.jumpFlag)
        {
            Physics.SetJumpForce(Movement.jumpDampTimer < PlayerMovement.JumpDampDuration ? Movement.jumpSpeed * 0.7f : Movement.jumpSpeed);
        
            Movement.OnJump();

            Inputs.ClearJumpFlag();
        }

        if (!onGround)
        {
            Physics.SetGroundMoveForce( Inputs.horizontalInput * Movement.moveSpeed );

            Movement.ResetGroundedTimers();
        }
        else
        {
            Physics.SetGroundMoveForce( Physics.velocityX * Mathf.Pow(1f - Mathf.Clamp01(Movement.stopTimer / Movement.stopDuration), 1f) ); // Stop moving

            if (Inputs.horizontalInput != 0f && Movement.hopTimer > Movement.hopDelay)
            {
                Movement.OnWalkHop();
                
                Physics.SetJumpForce(Movement.hopSpeed);
            }
        }
        
        Physics.AddForce(Physics2D.gravity * Time.fixedDeltaTime * (
            Physics.velocityY > 0 ? Movement.upGravityScale : Movement.downGravityScale));

        Physics.MovePlayer();

        Animation.FaceDirection(Inputs.horizontalInput);
    }

    private void Update()
    {
        Inputs.UpdateTimers();
    }

    private void LateUpdate()
    {
        ReadInputs();

        Collision.IncrementPhaseTimers();
        Movement.IncrementJumpTimers();
    }

    private void ReadInputs()
    {
        if (Inputs.verticalInput >= 0f)
            Collision.ResetPhaseTimers();

        if (Collision.onPhasablePlatform && !Collision.phasing && Inputs.verticalInput < -0.35f &&
            Collision.platformPhaseTimer >= PlayerCollision.PlatformPhaseHoldDuration)
        {
            Collision.StartCoroutine(Collision.PhaseThroughPlatforms(0.1f));
            Physics.SetForce(new Vector2(Physics.velocityX, -10f));
        }
        else if (Inputs.attackFlag)
        {
            if (Attack.attackTimer > Attack.AttackDuration)
            {
                Vector2 direction = Vector2.right * (Inputs.movementInputActive > 0f ? 1f : -1f);

                List<DetectedComponent> enemies = Attack.DetectEnemyHealthComponents(direction);
                Attack.AttackObjects(enemies, transform.position, direction, CalculateAttackDamage());
            }

            Inputs.ClearBlockFlag();

            if (Health.blocking)
            {
                Health.StopBlocking();
            }

            Movement.ResetJumpTimers();
        }
        else if (Attack.attackTimer > Attack.AttackDuration)
        {
            if (Attack.attacking)
            {
                Attack.StopAttack();
            }

            if (enableBlocking && Inputs.blockFlag)
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
        return Physics.velocityY < Physics2D.gravity.y * Movement.downGravityScale * Attack.fallingThreshold ?
        PlayerAttack.BaseDamage + Attack.fallingExtraDamage :
        PlayerAttack.BaseDamage;
    }
}
