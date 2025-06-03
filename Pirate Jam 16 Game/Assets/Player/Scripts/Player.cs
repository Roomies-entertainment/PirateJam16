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

public class Player : MonoBehaviour, IProcessExplosion
{
    [SerializeField] private bool enableBlocking;

    private PlayerInputs Inputs;
    private PlayerCollision Collision;
    private PlayerMovement Movement;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private PlayerAnimation Animation;
    private PlayerHealth Health;

    public void ProcessExplosion(Explosion explosion) { Physics.SyncForces(); Physics.ScaleSpeed(1.3f); }

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

    #region Fixed Update
    private void FixedUpdate()
    {
        bool onGroundFlag = Collision.GroundDetector.surfaceDetected;
        bool groundHitFlag = Collision.GroundDetector.gotHit;
        bool onWallFlag = Collision.GetOnWall(out ContactPoint2D wallContact);
        bool hopFlag = Inputs.horizontalInput != 0f && Movement.hopTimer > Movement.hopDelay;
        bool jumpFlag = (onGroundFlag || groundHitFlag && Physics.speedY > 0f) && Inputs.jumpFlag;

        FixedUpdatePhysics(onGroundFlag, onWallFlag, hopFlag, jumpFlag, wallContact);
        FixedUpdateMovement(onGroundFlag, hopFlag, jumpFlag);
        FixedUpdateInputs(jumpFlag);

        Animation.FaceDirection(Inputs.horizontalInput);

        Collision.IncrementTimers(Time.fixedDeltaTime);
        Movement.IncrementGroundedTimers();
    }

    private void FixedUpdatePhysics(bool onGroundFlag, bool onWallFlag, bool hopFlag, bool jumpFlag, ContactPoint2D wallContact)
    {
        if (!Physics.enabled)
        {
            return;
        }

        Physics.SyncForces();

        if (onGroundFlag)
        {
            Physics.AddVerticalSpeed(Physics2D.gravity.y * Movement.downGravityScale * Time.fixedDeltaTime);
            Physics.ClampVerticalSpeed(Physics2D.gravity.y * Movement.downGravityScale, Mathf.Infinity);

            if (hopFlag)
            {
                Physics.EnforceVerticalSpeed(Movement.hopVerticalSpeed);
            }
        }
        else
        {
            Physics.EnforceHorizontalSpeed(Inputs.horizontalInput * Movement.moveSpeed);

            if (Inputs.horizontalInput == 0f)
            {
                Physics.AddHorizontalSpeed(-Physics.speedX * Movement.slowSpeed * Time.fixedDeltaTime);
            }

            Physics.AddVerticalSpeed(Physics2D.gravity.y  * Time.fixedDeltaTime * (
                Physics.speedY > 0 ? Movement.upGravityScale : Movement.downGravityScale));

            if (onWallFlag)
            {
                Physics.SlideAlongSurface(wallContact.normal);
            }
        }

        if (jumpFlag)
        {
            Physics.EnforceVerticalSpeed(Movement.jumpDampTimer < PlayerMovement.JumpDampDuration ? Movement.jumpSpeed * 0.7f : Movement.jumpSpeed);
        }

        Physics.MovePlayer();
    }

    private void FixedUpdateMovement(bool onGroundFlag, bool hopFlag, bool jumpFlag)
    {
        if (!Movement.enabled)
        {
            return;
        }

        if (onGroundFlag)
        {
            if (hopFlag)
            {
                Movement.OnWalkHop();
            }
        }
        else
        {
            Movement.ResetGroundedTimers();
        }

        if (jumpFlag)
        {
            Movement.OnJump();
        }
    }

    private void FixedUpdateInputs(bool jumpFlag)
    {
        if (!Inputs.enabled)
        {
            return;
        }

        if (jumpFlag)
        {
            Inputs.ClearJumpFlag();
        }
    }
    #endregion

    #region Update
    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        if (!Inputs.enabled)
        {
            return;
        }

        Inputs.UpdateTimers();
    }
    #endregion

    #region Late Update
    private void LateUpdate()
    {
        bool platformPhaseFlag = (
            Collision.GetOnPhasablePlatform(out ContactPoint2D phasableContact) &&
            Collision.platformPhaseState == PlayerCollision.PlatformPhaseState.None &&
            Inputs.verticalInput < -0.35f &&
            Collision.platformPhaseHoldTimer >= PlayerCollision.PlatformPhaseHoldDuration);

        LateUpdatePhysics(platformPhaseFlag);
        LateUpdateCollision(platformPhaseFlag, phasableContact);
        LateUpdateHealth();
        LateUpdateMovement();
        LateUpdateAttack();
        LateUpdateInputs();

        Movement.IncrementJumpTimers();
    }

    private void LateUpdatePhysics(bool platformPhaseFlag)
    {
        if (!Physics.enabled)
        {
            return;
        }


        if (platformPhaseFlag)
        {
            Physics.EnforceVerticalSpeed(-Movement.fallThroughPlatformSpeed);
        }
    }

    private void LateUpdateCollision(bool platformPhaseFlag, ContactPoint2D phasableContact)
    {
        if (!Collision.enabled)
        {
            return;
        }


        if (Inputs.verticalInput >= 0f)
        {
            Collision.ResetPhaseTimers();

            if (Collision.platformPhaseState > PlayerCollision.PlatformPhaseState.ForcePhasing)
            {
                Collision.StopPhasingThroughPlatforms();
            }
        }
        else
        {
            if (platformPhaseFlag)
            {
                Collision.StartPhasingThroughPlatforms(phasableContact.collider, 0.1f);
            }
        }
    }

    private void LateUpdateHealth()
    {
        if (!Health.enabled)
        {
            return;
        }


        if (Inputs.attackFlag)
        {
            if (Health.blocking)
            {
                Health.StopBlocking();
            }
        }
        else if (Attack.attackTimer > Attack.AttackDuration)
        {
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

    private void LateUpdateMovement()
    {
        if (!Inputs.enabled)
        {
            return;
        }


        if (Inputs.attackFlag)
        {
            Movement.ResetJumpTimers();
        }
    }

    private void LateUpdateAttack()
    {
        if (!Attack.enabled)
        {
            return;
        }
        

        if (Inputs.attackFlag)
        {
            if (!Attack.attacking)
            {
                Attack.SetAttackDirection(Vector2.right * (Inputs.movementInputActive > 0f ? 1f : -1f));
            }

            Attack.FindComponents();
            Attack.AttackAndInteract();
        }
        else if (Attack.attackTimer > Attack.AttackDuration)
        {
            if (Attack.attacking)
            {
                Attack.StopAttack();
            }
        }
    }

    private void LateUpdateInputs()
    {
        if (!Inputs.enabled)
        {
            return;
        }

        if (Inputs.attackFlag)
        {
            Inputs.ClearBlockFlag();
        }
    }
    #endregion

    private int CalculateAttackDamage()
    {
        return (
            !Collision.GroundDetector.surfaceDetected &&
            Physics.speedY < Physics2D.gravity.y * Movement.downGravityScale * Attack.fallingThreshold ?
                PlayerAttack.BaseDamage + Attack.fallingExtraDamage :
                PlayerAttack.BaseDamage);
    }
}
