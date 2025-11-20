using UnityEngine;

public class Player : Controller
{
    // inspector assigned
    [Header("")]
    [SerializeField] private PlayerInputs Inputs;
    [SerializeField] private PlayerColliders Colliders;
    [SerializeField] private PlayerUI UI;

    private PlayerCollision Collision;

    private PlayerSurfaceDetector GroundDetector;
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
        Collision       = Components.GetComponent<PlayerCollision>();
        GroundDetector  = Components.GetComponent<PlayerSurfaceDetector>();
        Movement        = Components.GetComponent<PlayerMovement>();
        Physics         = Components.GetComponent<PlayerPhysics>();
        Attack          = Components.GetComponent<PlayerAttack>();
        Health          = Components.GetComponent<PlayerHealth>();
        Animation       = Components.GetComponent<PlayerAnimation>();
        Particles       = Components.GetComponent<PlayerParticles>();
    }

    private void Start()
    {
        Physics.InitializeRigidbody();

        if (UI != null) StartUI();
    }

    #region Start
    private void StartUI()
    {
        UI.UpdateHealthBar((float) Health.health / Health.maxHealth);
        UI.counterText.text = UI.deathCounter.ToString();
    }
    #endregion

    #region Fixed Update
    private void FixedUpdate()
    {
        bool onGroundFlag           = GroundDetector.surfaceDetected && Physics.speedY <= 0;
        bool isGroundFlag           = onGroundFlag || (GroundDetector.gotHit && Physics.speedY > 0f);

        bool onWallFlag             = Collision.GetOnWall(out ContactPoint2D wallContact);
        bool onCeilingFlag          = Collision.GetOnCeiling(out ContactPoint2D ceilingContact);

        bool hopFlag                = Inputs.horizontalInput != 0f && Movement.hopTimer > Movement.hopDelay;
        bool jumpFlag               = Inputs.jumpFlag && (isGroundFlag || (!onGroundFlag && Movement.jumpHoldTimer < Movement.jumpHoldDuration));

        if (Movement.enabled)       FixedUpdateMovement(onGroundFlag, hopFlag, jumpFlag);
        if (Physics.enabled)        FixedUpdatePhysics(onGroundFlag, onWallFlag, onCeilingFlag, wallContact, ceilingContact, hopFlag, jumpFlag);
        if (Animation.enabled)      FixedUpdateAnimation();
        
        if (Collision.enabled)      LateFixedUpdateCollision();
    }

    private void FixedUpdateMovement(bool onGroundFlag, bool hopFlag, bool jumpFlag)
    {
        float speedX = Inputs.horizontalInput * Movement.moveSpeed;
        float speedY;

        if (jumpFlag && onGroundFlag)
        {
            Movement.ResetJumpTimers();
            Movement.OnJump();
        }

        if (jumpFlag)
            speedY = Movement.jumpSpeed * (Attack.attackFlag ? 0.4f : 1.0f);
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

        Movement.IncrementGroundedTimers();
    }

    private void FixedUpdatePhysics(bool onGroundFlag, bool onWallFlag, bool onCeilingFlag, ContactPoint2D wallContact, ContactPoint2D ceilingContact, bool hopFlag, bool jumpFlag)
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
                Physics.AddHorizontalSpeed(-Physics.speedX * Movement.aerialSlowSpeed * Time.fixedDeltaTime);
            }

            Physics.AddVerticalSpeed(Physics2D.gravity.y * Time.fixedDeltaTime * (
                Physics.speedY > 0 ? Movement.upGravityScale : Movement.downGravityScale));

            if (onWallFlag)
            {
                Physics.SlideAlongSurface(wallContact.normal);
            }
            else if (onCeilingFlag)
            {
                Physics.SlideAlongSurface(ceilingContact.normal);
            }
        }

        if (jumpFlag)
        {
            Physics.EnforceVerticalSpeed(Movement.speedY);
        }

        Physics.MovePlayer();
    }

    private void FixedUpdateAnimation()
    {
        if (Mathf.Abs(Physics.speedX) > 0.1f)
        {
            Animation.FaceDirection(Physics.speedX);
        }
    }

    private void LateFixedUpdateCollision()
    {
        Collision.IncrementTimers(Time.fixedDeltaTime);
    }
    #endregion

    #region Update
    private void Update()
    {
        if (Attack.enabled)         UpdateAttack();
    }

    private void UpdateAttack()
    {        
        if (Inputs.attackFlag)
        {
            Attack.SetAttackDirection(Vector2.right * (Inputs.movementInputActive > 0f ? 1f : -1f));
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
    #endregion

    #region Late Update
    private void LateUpdate()
    {
        bool platformPhaseFlag = (
            Collision.GetOnPhasablePlatform(out ContactPoint2D phasableContact) &&
            Collision.platformPhaseState == PlayerCollision.PlatformPhaseState.None &&
            Inputs.verticalInput < -0.35f &&
            Collision.platformPhaseHoldTimer >= PlayerCollision.PlatformPhaseHoldDuration);

        if (Movement.enabled)       LateUpdateMovement(platformPhaseFlag);
        if (Physics.enabled)        LateUpdatePhysics(platformPhaseFlag);
        if (Collision.enabled)      LateUpdateCollision(platformPhaseFlag, phasableContact);
        if (Collision.enabled)      LateUpdateCollision();
        if (GroundDetector.enabled) LateUpdateGroundDetector();
        if (Health.enabled)         LateUpdateHealth();
        if (Particles.enabled)      LateUpdateParticles();
        if (Animation.enabled)      LateUpdateAnimation();

        if (UI != null &&
            UI.enabled)             LateUpdateUI();


                                    Attack.ClearFlags();

        if (Health.deathFlag)         gameObject.SetActive(false);
        
                                    
                                    Health.ClearUpdate();

    }
    private void LateUpdateMovement(bool platformPhaseFlag)
    {
        if (platformPhaseFlag)
        {
            Movement.speedY = -Movement.fallThroughPlatformSpeed;
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

        Health.ProcessHealthEvents();
    }

    private void LateUpdateCollision()
    {
        if (Attack.startAttackFlag)
        {
            Colliders.SetFlatConfiguration();
        }
        else if (Attack.stopAttackFlag)
        {
            Colliders.SetUprightConfiguration();
        }
    }

    private void LateUpdateGroundDetector()
    {
        if (Attack.startAttackFlag)
        {
            GroundDetector.SetFlatConfiguration();
        }
        else if (Attack.stopAttackFlag)
        {
            GroundDetector.SetUprightConfiguration();
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
        if (Health.healthChangeFlag)
        {
            UI.UpdateHealthBar((float) Health.health / Health.maxHealth);
        }

        if (Health.deathFlag)
        {
            UI.IncreaseDeathCounter();
            UI.SetDeathScreenActive(true);
        }
    }
    #endregion
}
