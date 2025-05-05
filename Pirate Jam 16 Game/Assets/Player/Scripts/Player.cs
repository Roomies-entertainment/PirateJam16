using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerHealth))]

public class Player : MonoBehaviour
{
    private PlayerInput Input;
    private PlayerCollision Collision;
    private PlayerMovement Movement;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private PlayerAnimation Animation;
    private PlayerHealth Health;

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

        Movement.IncrementGroundedTimers();
    }

    private void HandleMovement()
    {
        bool farHit = Collision.GroundDetector.gotHit;
        bool onGround = Collision.GroundDetector.surfaceDetected;

        if ((onGround || farHit && Physics.velocityY > 0f) && Input.jumpFlag)
        {
            Physics.SetJumpForce(Movement.jumpDampTimer < PlayerMovement.JumpDampDuration ? Movement.jumpSpeed * 0.7f : Movement.jumpSpeed);
        
            Movement.OnJump();

            Input.ClearJumpFlag();
        }

        if (!onGround)
        {
            Physics.SetGroundMoveForce( Input.movementInput * Movement.moveSpeed );

            Movement.ResetGroundedTimers();
        }
        else
        {
            Physics.SetGroundMoveForce( Physics.velocityX * Mathf.Pow(1f - Mathf.Clamp01(Movement.stopTimer / Movement.stopDuration), 1f) ); // Stop moving

            if (Input.movementInput != 0f && Movement.hopTimer > Movement.hopDelay)
            {
                Movement.OnWalkHop();
                
                Physics.SetJumpForce(Movement.hopSpeed);
            }
        }
        
        Physics.AddForce(Physics2D.gravity * Time.fixedDeltaTime * (
            Physics.velocityY > 0 ? Movement.upGravityScale : Movement.downGravityScale));

        Physics.MovePlayer();

        Animation.FaceDirection(Input.movementInput);
    }

    private void Update()
    {
        Input.GetInputs();
        Input.UpdateTimers();
    }

    private void LateUpdate()
    {
        ReadInputs();

        Collision.IncrementPhaseTimers();
        Movement.IncrementJumpTimers();
    }

    private void ReadInputs()
    {
        if (Input.verticalInput >= 0f)
            Collision.ResetPhaseTimers();

        if (Collision.onPhasablePlatform && !Collision.phasing && Input.verticalInput < -0.35f &&
            Collision.platformPhaseTimer >= PlayerCollision.PlatformPhaseHoldDuration)
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

            Movement.ResetJumpTimers();
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
        return Physics.velocityY < Physics2D.gravity.y * Movement.downGravityScale * Attack.fallingThreshold ?
        PlayerAttack.BaseDamage + Attack.fallingExtraDamage :
        PlayerAttack.BaseDamage;
    }
}
