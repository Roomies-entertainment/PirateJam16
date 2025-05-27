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

        Collision.IncrementTimers(Time.fixedDeltaTime);
        Movement.IncrementGroundedTimers();
    }

    private void HandleMovement()
    {
        bool farHit = Collision.GroundDetector.gotHit;
        bool onGround = Collision.GroundDetector.surfaceDetected;

        if ((onGround || farHit && Physics.speedY > 0f) && Inputs.jumpFlag)
        {
            Physics.SetVerticalSpeed(Movement.jumpDampTimer < PlayerMovement.JumpDampDuration ? Movement.jumpSpeed * 0.7f : Movement.jumpSpeed);
        
            Movement.OnJump();

            Inputs.ClearJumpFlag();
        }

        if (!onGround)
        {
            Physics.SetHorizontalSpeed( Inputs.horizontalInput * Movement.moveSpeed );
            Physics.AddForce(Physics2D.gravity * (
                Physics.speedY > 0 ? Movement.upGravityScale : Movement.downGravityScale));
            
            if (Collision.GetOnWall(out ContactPoint2D contactPoint))
            {
                Physics.SlideAlongSurface(contactPoint.normal);
            }

            Movement.ResetGroundedTimers();
        }
        else
        {
            Physics.SetHorizontalSpeed( Physics.speedX * ( 1f - Mathf.Clamp01(Movement.stopTimer / Movement.stopDuration) ) ); // Stop moving

            Physics.AddForce(Vector2.up * Physics2D.gravity.y * Movement.downGravityScale);
            Physics.ClampVerticalSpeed(Physics2D.gravity.y * Movement.downGravityScale, Mathf.Infinity);

            if (Inputs.horizontalInput != 0f && Movement.hopTimer > Movement.hopDelay)
            {
                Movement.OnWalkHop();
                
                Physics.SetVerticalSpeed(Movement.hopVerticalSpeed);
            }
        }

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

        Movement.IncrementJumpTimers();
    }

    private void ReadInputs()
    {
        if (Inputs.verticalInput >= 0f)
            Collision.ResetPhaseTimers();
        else
        {
            if (Collision.platformPhaseState > PlayerCollision.PlatformPhaseState.ForcePhasing)
            {
                Collision.StopPhasingThroughPlatforms();
            }
        }

        if (Collision.GetOnPhasablePlatform(out ContactPoint2D contactPoint) &&
            Collision.platformPhaseState == PlayerCollision.PlatformPhaseState.None &&
            Inputs.verticalInput < -0.35f &&
            Collision.platformPhaseHoldTimer >= PlayerCollision.PlatformPhaseHoldDuration)
        {
            Collision.StartPhasingThroughPlatforms(contactPoint.collider, 0.1f);
            Physics.SetVerticalSpeed(-Movement.fallThroughPlatformSpeed);
        }
        else if (Inputs.attackFlag)
        {
            if (!Attack.attacking)
            {
                Attack.SetAttackDirection(Vector2.right * (Inputs.movementInputActive > 0f ? 1f : -1f));
            }
            
            Attack.FindComponents(out var healthComponents, out var interactables);
            var damage = CalculateAttackDamage();

            Attack.PerformAttack(healthComponents, damage);
            Attack.PerformInteractions(interactables);

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
        return (
            !Collision.GroundDetector.surfaceDetected &&
            Physics.speedY < Physics2D.gravity.y * Movement.downGravityScale * Attack.fallingThreshold ?
                PlayerAttack.BaseDamage + Attack.fallingExtraDamage :
                PlayerAttack.BaseDamage);
    }
}
