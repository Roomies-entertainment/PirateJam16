using UnityEngine;

public class RangerBahaviour : Behaviour
{
    private GroundDetection GroundDetection;
    private HorizontalMovement HorizontalMovement;
    private JumpMovement JumpMovement;
    private EnemyAttack Attack;
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
    private float attackDelay, attackDuration;

    private void RandomizeAttackDelay() { attackDelay = RandomM.Range(attackDelayRange.x, attackDelayRange.y); }
    private void RandomizeAttackDuration() { attackDuration = RandomM.Range(attackDurationRange.x, attackDurationRange.y); }

    private float attackLoopTimer;

    private enum AttackState
    {
        Null,
        None,
        Attack,
        Attacking
    }

    private AttackState attackState;

    private void SetAttackState(AttackState setTo)
    {
        if (setTo == AttackState.Attack)
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
                Attack.SetAttackDirection(Attack.GetAttackDirection());
                Attack.AttackAndInteract();
                transform.position += new Vector3(Attack.attackDirection.x, Attack.attackDirection.y, 0f) * 0.25f;

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
        Attack = Components.GetComponentInChildren<EnemyAttack>();
        Health = Components.GetComponentInChildren<EnemyHealth>();
        Animation = Components.GetComponentInChildren<EnemyAnimation>();
        Particles = Components.GetComponentInChildren<EnemyParticles>();
    }

    private void Start()
    {
        SetAttackState(AttackState.Attack);
    }

    private void FixedUpdate()
    {
        if (HorizontalMovement.enabled)
            FixedUpdateHorizontalMovement();
    }

    private void FixedUpdateHorizontalMovement()
    {
        HorizontalMovement.MoveHorizontally();
    }

    private void Update()
    {
        if (HorizontalMovement.enabled)
            UpdateHorizontalMovement();

        if (GroundDetection.enabled)
            UpdateEnemyDetection();

        if (JumpMovement.enabled)
            UpdateJumpMovement();

        if (Attack.enabled)
            UpdateAttackState();

        attackStateChange[0] = AttackState.Null;
        attackStateChange[1] = AttackState.Null;
    }

    private void UpdateHorizontalMovement()
    {
        if (HorizontalMovement.currentSpeed == 0)
        {
            if (moveTimer > moveDelay)
            {
                HorizontalMovement.StartMoving(RandomM.Float0To1() < walkBackwardsChance ? -moveSpeed : moveSpeed);

                moveTimer = 0.0f;
            }
        }
        else
        {
            if (GroundDetection.GroundCheck.check && moveTimer > moveDuration)
            {
                HorizontalMovement.StopMoving();
                moveTimer = 0.0f;
            }
        }

        if (GroundDetection.GroundCheck.check && GroundDetection.EdgeChecks.exitFlag)
        {
            HorizontalMovement.MoveAwayFromCurrentDirection();
        }

        moveTimer += Time.deltaTime;
    }

    private void UpdateEnemyDetection()
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

    private void UpdateAttackState()
    {
        switch (attackState)
        {
            case AttackState.Attack:

                if (Attack.FindComponents() && attackLoopTimer > attackDelay)
                    SetAttackState(AttackState.Attacking);

                break;

            case AttackState.Attacking:

                if (attackLoopTimer > attackDuration)
                {
                    SetAttackState(AttackState.Attack);
                }

                break;
        }

        attackLoopTimer += Time.deltaTime;
    }
}
