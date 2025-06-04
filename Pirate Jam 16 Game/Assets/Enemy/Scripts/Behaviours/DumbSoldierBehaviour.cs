using UnityEngine;

public class DumbSoldierBehaviour : Behaviour
{
    private GroundDetection GroundDetection;
    private HorizontalMovement HorizontalMovement;
    private JumpMovement JumpMovement;
    private EnemyMeleeAttack Attack;
    private EnemyHealth Health;
    private EnemyAnimation Animation;
    private EnemyParticles Particles;

    [Header("Movement Loop")]
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float moveDelay = 3.0f;
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField][Range(0, 1)] private float walkBackwardsChance = 0.3f;
    private float moveTimer;

    [Header("Attack Loop")]
    [Space]
    [SerializeField] private Vector2 attackDelayRange = new Vector2(0.4f, 0.8f);
    [SerializeField] private Vector2 attackDurationRange = new Vector2(0.3f, 0.4f);
    [SerializeField] private Vector2 blockDelayRange = new Vector2(0.3f, 0.4f);
    [SerializeField] private Vector2 blockDurationRange = new Vector2(1f, 1.5f);
    [SerializeField][Range(0, 1)] private float blockChance = 0.5f;
    /* 
            [Header("")]
            [SerializeField] private bool debug;
         */
    private float attackDelay, attackDuration, blockDelay, blockDuration;

    private void RandomizeAttackDelay() { attackDelay = RandomM.Range(attackDelayRange.x, attackDelayRange.y); }
    private void RandomizeAttackDuration() { attackDuration = RandomM.Range(attackDurationRange.x, attackDurationRange.y); }
    private void RandomizeBlockDelay() { blockDelay = RandomM.Range(blockDelayRange.x, blockDelayRange.y); }
    private void RandomizeBlockDuration() { blockDuration = RandomM.Range(blockDurationRange.x, blockDurationRange.y); }

    private float attackLoopTimer;

    private enum AttackState
    {
        Null,
        None,
        Attack,
        Attacking,
        Block,
        Blocking
    }

    private AttackState attackState;

    private void SetAttackState(AttackState setTo)
    {
        if (setTo == AttackState.Attack || setTo == AttackState.Block)
        {
            Attack.StopAttack();
        }

        switch (setTo)
        {
            case AttackState.Attack:

                RandomizeAttackDelay();

                break;

            case AttackState.Attacking:

                RandomizeAttackDuration();

                break;

            case AttackState.Block:

                RandomizeBlockDelay();

                break;

            case AttackState.Blocking:

                RandomizeBlockDuration();

                break;
        }

        attackStateChange[0] = attackState;
        attackStateChange[1] = setTo;

        attackState = setTo;

        attackLoopTimer = 0f;
    }

    private AttackState[] attackStateChange = new AttackState[2];

    private void Awake()
    {
        GroundDetection = Components.GetComponentInChildren<GroundDetection>();
        HorizontalMovement = Components.GetComponentInChildren<HorizontalMovement>();
        JumpMovement = Components.GetComponentInChildren<JumpMovement>();
        Attack = Components.GetComponentInChildren<EnemyMeleeAttack>();
        Health = Components.GetComponentInChildren<EnemyHealth>();
        Animation = Components.GetComponentInChildren<EnemyAnimation>();
        Particles = Components.GetComponentInChildren<EnemyParticles>();
    }

    private void Start()
    {
        SetAttackState(AttackState.Attack);
    }

    #region FixedUpdate
    private void FixedUpdate()
    {
        if (HorizontalMovement.enabled)
            FixedUpdateHorizontalMovement();
    }

    private void FixedUpdateHorizontalMovement()
    {
        HorizontalMovement.MoveHorizontally();
    }
    #endregion

    #region Update
    private void Update()
    {
        if (GroundDetection.enabled)
            UpdateGroundDetection();

        if (Attack.enabled)
            UpdateAttack();

        if (HorizontalMovement.enabled)
            UpdateHorizontalMovement();

        if (JumpMovement.enabled)
            UpdateJumpMovement();

        if (Health.enabled)
            UpdateHealth();

        attackStateChange[0] = AttackState.Null;
        attackStateChange[1] = AttackState.Null;

        moveTimer += Time.deltaTime;
    }

    private void UpdateHorizontalMovement()
    {
        if (HorizontalMovement.currentSpeed == 0)
        {
            if (moveTimer > moveDelay)
            {
                HorizontalMovement.SetFaceDirection(Random.value > 0.5f ? Vector2.left : Vector2.right);
                HorizontalMovement.StartMoving(RandomM.Float0To1() < walkBackwardsChance ? -moveSpeed : moveSpeed);

                moveTimer = 0.0f;
            }
        }
        else
        {
            if (GroundDetection.GroundCheck.check && GroundDetection.EdgeChecks.exitFlag)
            {
                HorizontalMovement.MoveAwayFromCurrentDirection();
            }

            if (GroundDetection.GroundCheck.check && moveTimer > moveDuration)
            {
                HorizontalMovement.StopMoving();
                moveTimer = 0.0f;
            }
        }

        if (Attack.startAttackFlag)
        {
            HorizontalMovement.SetFaceDirection(Attack.attackDirection);
        }
    }

    private void UpdateGroundDetection()
    {
        if (HorizontalMovement.startMovingFlag)
        {
            Vector2 moveDir = HorizontalMovement.moveDirection;

            GroundDetection.StepChecks.SetCheckDir(moveDir);
            GroundDetection.StepClearanceChecks.SetCheckDir(moveDir);
        }
    }

    private void UpdateJumpMovement()
    {
        if (GroundDetection.GroundCheck.check &&
            GroundDetection.StepChecks.enterFlag &&
            (GroundDetection.StepClearanceChecks.checkCount == 0) &&
            HorizontalMovement.currentSpeed != 0)
        {
            JumpMovement.Jump();
        }
    }

    private void UpdateAttack()
    {
        switch (attackState)
        {
            case AttackState.Attack:

                if (Attack.FindComponents() && attackLoopTimer > attackDelay)
                {
                    SetAttackState(AttackState.Attacking);

                    Attack.SetAttackDirection(Attack.GetAttackDirection());
                    Attack.PerformAttack();
                    transform.position += new Vector3(Attack.attackDirection.x, Attack.attackDirection.y, 0f) * 0.25f;
                }

                break;

            case AttackState.Attacking:

                if (attackLoopTimer > attackDuration)
                {
                    if (RandomM.Float0To1() < blockChance)
                        SetAttackState(AttackState.Attack);
                    else
                        SetAttackState(AttackState.Block);

                }

                break;

            case AttackState.Block:

                if (attackLoopTimer > blockDelay)
                    SetAttackState(AttackState.Blocking);

                break;

            case AttackState.Blocking:

                if (attackLoopTimer > blockDuration)
                    SetAttackState(AttackState.Attack);

                break;
        }

        attackLoopTimer += Time.deltaTime;
    }

    private void UpdateHealth()
    {
        if (attackStateChange[0] != attackStateChange[1])
        {
            if (!Health.blocking && attackStateChange[1] == AttackState.Blocking)
            {
                Health.SetBlockDirection(Attack.attackDirection);
                Health.StartBlocking();
            }
            else if (Health.blocking && attackStateChange[1] != AttackState.Blocking)
            {
                Health.StopBlocking();
            }
        }
    }
    #endregion
}
