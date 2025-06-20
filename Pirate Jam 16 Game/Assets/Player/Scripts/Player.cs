using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerParticles))]

public class Player : MonoBehaviour
{
    [SerializeField] private bool enableBlocking;

    private PlayerInputs Inputs;
    private PlayerCollision Collision;
    private PlayerMovement Movement;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private PlayerHealth Health;
    private PlayerAnimation Animation;
    private PlayerParticles Particles;

    public void SetGameplayEnabled(bool setTo)
    {
        Attack.enabled = setTo;
        Movement.enabled = setTo;
    }

    public void ProcessProjectile(Projectile projectile) { }

    private void Awake()
    {
        Inputs = GetComponent<PlayerInputs>();
        Collision = GetComponent<PlayerCollision>();
        Movement = GetComponent<PlayerMovement>();
        Physics = GetComponent<PlayerPhysics>();
        Attack = GetComponent<PlayerAttack>();
        Health = GetComponent<PlayerHealth>();
        Animation = GetComponent<PlayerAnimation>();
        Particles = GetComponent<PlayerParticles>();

        StaticReferences.playerReference = this;
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

        FixedUpdateMovement(onGroundFlag, hopFlag, jumpFlag);
        FixedUpdatePhysics(onGroundFlag, onWallFlag, hopFlag, jumpFlag, wallContact);
        FixedUpdateInputs(jumpFlag);
        FixedUpdateAnimation();
        FixedUpdateCollision();
    }

    private void FixedUpdatePhysics(bool onGroundFlag, bool onWallFlag, bool hopFlag, bool jumpFlag, ContactPoint2D wallContact)
    {
        if (!Physics.enabled)
        {
            return;
        }

        //Physics.SyncForces();
        
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
            Physics.EnforceHorizontalSpeed(Movement.speedX);

            if (Inputs.horizontalInput == 0f)
            {
                Physics.AddHorizontalSpeed(-Physics.speedX * Movement.slowSpeed * Time.fixedDeltaTime);
            }

            Physics.AddVerticalSpeed(Physics2D.gravity.y * Time.fixedDeltaTime * (
                Physics.speedY > 0 ? Movement.upGravityScale : Movement.downGravityScale));

            if (onWallFlag)
            {
                Physics.SlideAlongSurface(wallContact.normal);
            }
        }

        if (jumpFlag)
        {
            Physics.EnforceVerticalSpeed(Movement.speedY);
        }

        Physics.MovePlayer();
    }

    private void FixedUpdateMovement(bool onGroundFlag, bool hopFlag, bool jumpFlag)
    {
        if (!Movement.enabled)
        {
            return;
        }

        float speedX = Inputs.horizontalInput * Movement.moveSpeed;
        float speedY;

        if (jumpFlag)
            speedY = Movement.jumpDampTimer < PlayerMovement.JumpDampDuration ? Movement.jumpSpeed * 0.7f : Movement.jumpSpeed;
        else
            speedY = Physics.speedY;

        Movement.SetSpeeds(speedX, speedY);

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

        Movement.IncrementGroundedTimers();
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

    private void FixedUpdateAnimation()
    {
        if (!Animation.enabled)
        {
            return;
        }

        if (Mathf.Abs(Physics.speedX) > 0.1f)
        {
            Animation.FaceDirection(Physics.speedX);
        }
    }

    private void FixedUpdateCollision()
    {
        if (!Collision.enabled)
        {
            return;
        }

        Collision.IncrementTimers(Time.fixedDeltaTime);
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
            Inputs.ResetTimers();

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

        LateUpdateMovement(platformPhaseFlag);
        LateUpdatePhysics(platformPhaseFlag);
        LateUpdateCollision(platformPhaseFlag, phasableContact);
        LateUpdateAttack();
        LateUpdateHealth();
        LateUpdateInputs();
        LateUpdateParticles();
    }

    private void LateUpdateMovement(bool platformPhaseFlag)
    {
        if (!Movement.enabled)
        {
            Movement.ResetGroundedTimers();
            Movement.ResetJumpTimers();
            
            return;
        }

        if (platformPhaseFlag)
        {
            Movement.speedY = -Movement.fallThroughPlatformSpeed;
        }

        if (Inputs.attackFlag)
        {
            Movement.ResetJumpTimers();
        }

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
            Physics.EnforceVerticalSpeed(Movement.speedY);
        }
    }

    private void LateUpdateCollision(bool platformPhaseFlag, ContactPoint2D phasableContact)
    {
        if (!Collision.enabled)
        {
            Collision.ResetPhaseTimers();

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
        else if (Movement.speedY < 0 && platformPhaseFlag)
        {
            Collision.StartPhasingThroughPlatforms(phasableContact.collider, 0.1f);
        }
    }

    private void LateUpdateHealth()
    {
        if (!Health.enabled)
        {
            return;
        }

        if (Attack.startAttackFlag)
        {
            if (Health.blocking)
            {
                Health.StopBlocking();
            }

            Health.deflectProjectiles = true;
        }
        else
        {
            if (Attack.timeSinceAttack > Attack.AttackDuration)
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

            if (Health.deflectProjectiles && Attack.timeSinceAttack > Health.deflectionWindow)
            {
                Health.deflectProjectiles = false;
            }
        }

        print(Health.deflectProjectiles);
    }

    private void LateUpdateAttack()
    {
        if (!Attack.enabled)
        {
            return;
        }
        

        if (Inputs.attackFlag)
        {
            if (!Attack.attackFlag)
            {
                Attack.SetAttackDirection(Vector2.right * (Inputs.movementInputActive > 0f ? 1f : -1f));
            }

            Attack.FindComponents();
            Attack.PerformAttack();
        }
        else if (Attack.timeSinceAttack > Attack.AttackDuration)
        {
            if (Attack.attackFlag)
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

    private void LateUpdateParticles()
    {
        if (Attack.startAttackFlag)
        {
            Particles.MoveEffectParticles(
                Vector2.up * -0.2f + Attack.attackDirection * -0.4f/*idk why it's reversed*/, new Vector3(0f, 0f, 90f));
        }
        else if (Attack.stopAttackFlag)
        {
            Particles.MoveEffectParticles(Vector2.zero, Vector3.zero);
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
