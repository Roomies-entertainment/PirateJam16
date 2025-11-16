using UnityEngine;

public class Player : Behaviour
{
    private PlayerInputs Inputs;
    private PlayerCollision Collision;
    private PlayerMovement Movement;
    private PlayerPhysics Physics;
    private PlayerAttack Attack;
    private PlayerHealth Health;
    private PlayerAnimation Animation;
    private PlayerParticles Particles;

    [SerializeField] // inspector assigned
    private PlayerUI UIComponent;

    public void SetGameplayEnabled(bool setTo)
    {
        Attack.enabled = setTo;
        Movement.enabled = setTo;
    }

    public void ProcessProjectile(Projectile projectile) { }

    private void Awake()
    {
        Inputs      = Components.GetComponent<PlayerInputs>();
        Collision   = Components.GetComponent<PlayerCollision>();
        Movement    = Components.GetComponent<PlayerMovement>();
        Physics     = Components.GetComponent<PlayerPhysics>();
        Attack      = Components.GetComponent<PlayerAttack>();
        Health      = Components.GetComponent<PlayerHealth>();
        Animation   = Components.GetComponent<PlayerAnimation>();
        Particles   = Components.GetComponent<PlayerParticles>();

        StaticReferences.playerReference = this;
    }

    private void Start()
    {
        Physics.InitializeRigidbody();

        if (UIComponent != null) StartUI();
    }

    #region Start
    private void StartUI()
    {
        UIComponent.UpdateHealthBar((float) Health.health / Health.maxHealth);
        UIComponent.counterText.text = UIComponent.deathCounter.ToString();
    }
    #endregion

    #region Fixed Update
    private void FixedUpdate()
    {
        bool onGroundFlag       = Collision.GroundDetector.surfaceDetected && Physics.speedY <= 0;
        bool groundHitFlag      = Collision.GroundDetector.gotHit;
        bool onWallFlag         = Collision.GetOnWall(out ContactPoint2D wallContact);
        bool hopFlag            = Inputs.horizontalInput != 0f && Movement.hopTimer > Movement.hopDelay;
        bool jumpFlag           = (onGroundFlag || groundHitFlag && Physics.speedY > 0f) && Inputs.jumpFlag;

        if (Movement.enabled)   FixedUpdateMovement(onGroundFlag, hopFlag, jumpFlag);
        if (Physics.enabled)    FixedUpdatePhysics(onGroundFlag, onWallFlag, hopFlag, jumpFlag, wallContact);
        if (Inputs.enabled)     FixedUpdateInputs(jumpFlag);
        if (Animation.enabled)  FixedUpdateAnimation();
        if (Collision.enabled)  FixedUpdateCollision();
    }

    private void FixedUpdatePhysics(bool onGroundFlag, bool onWallFlag, bool hopFlag, bool jumpFlag, ContactPoint2D wallContact)
    {        
        if (onGroundFlag)
        {
            Physics.SyncForces();
            
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
        if (jumpFlag)
        {
            Inputs.ClearJumpFlag();
        }
    }

    private void FixedUpdateAnimation()
    {
        if (Mathf.Abs(Physics.speedX) > 0.1f)
        {
            Animation.FaceDirection(Physics.speedX);
        }
    }

    private void FixedUpdateCollision()
    {
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

        if (Movement.enabled)   LateUpdateMovement(platformPhaseFlag);
        if (Physics.enabled)    LateUpdatePhysics(platformPhaseFlag);
        if (Collision.enabled)  LateUpdateCollision(platformPhaseFlag, phasableContact);
        if (Attack.enabled)     LateUpdateAttack();
        if (Health.enabled)     LateUpdateHealth();
        if (Inputs.enabled)     LateUpdateInputs();
        if (Particles.enabled)  LateUpdateParticles();
        if (Animation.enabled)  LateUpdateAnimation();
        if (UIComponent != null &&
            UIComponent.enabled)         LateUpdateUI();

        if (Health.dieFlag)
        {
            gameObject.SetActive(false);
        }
    }

    private void LateUpdateMovement(bool platformPhaseFlag)
    {
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
        if (platformPhaseFlag)
        {
            Physics.EnforceVerticalSpeed(Movement.speedY);
        }
    }

    private void LateUpdateCollision(bool platformPhaseFlag, ContactPoint2D phasableContact)
    {
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
        if (Attack.startAttackFlag)
        {
            if (Health.blockFlag)
            {
                Health.StopBlocking();
            }

            Health.deflectProjectiles = true;
        }
        else
        {
            if (Health.deflectProjectiles && Attack.timeSinceAttack > Health.deflectionWindow)
            {
                Health.deflectProjectiles = false;
            }
        }
    }

    private void LateUpdateAttack()
    {        
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
                new Vector2(0f, -0.2f),
                new Vector3(0f, 0f, -90f));
        }
        else if (Attack.stopAttackFlag)
        {
            Particles.MoveEffectParticles(Vector2.zero, Vector3.zero);
        }

    }

    private void LateUpdateAnimation()
    {
        if (Attack.startAttackFlag)
        {
            Animation.PlayAttackAnimation();
        }
        else if (Attack.stopAttackFlag)
        {
            Animation.StopAttackAnimation();
        }
    }
    
    private void LateUpdateUI()
    {
        if (Health.takeDamageFlag || Health.healFlag)
        {
            UIComponent.UpdateHealthBar((float) Health.health / Health.maxHealth);
        }

        if (Health.dieFlag)
        {
            UIComponent.IncreaseDeathCounter();
            UIComponent.SetDeathScreenActive(true);
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
