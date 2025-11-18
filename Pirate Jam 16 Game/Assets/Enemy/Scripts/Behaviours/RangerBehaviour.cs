using UnityEngine;

public class RangerBehaviour : Controller
{
    private GroundDetection GroundDetection;
    private HorizontalMovement HorizontalMovement;
    private JumpMovement JumpMovement;
    private EnemyRangedAttack Attack;
    private EnemyHealth Health;
    private EnemyAnimation Animation;

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
        GroundDetection     = Components.GetComponentInChildren<GroundDetection>();
        HorizontalMovement  = Components.GetComponentInChildren<HorizontalMovement>();
        JumpMovement        = Components.GetComponentInChildren<JumpMovement>();
        Attack              = Components.GetComponentInChildren<EnemyRangedAttack>();
        Health              = Components.GetComponentInChildren<EnemyHealth>();
        Animation           = Components.GetComponentInChildren<EnemyAnimation>();
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
        HorizontalMovement.ApplyMovement();
    }

    private void Update()
    {
        if (GroundDetection.enabled)    UpdateGroundDetection();
        if (Attack.enabled)             UpdateAttack();
        if (HorizontalMovement.enabled) UpdateHorizontalMovement();
        if (JumpMovement.enabled)       UpdateJumpMovement();
        if (Animation.enabled)          UpdateAnimation();
        
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
                HorizontalMovement.FaceDirection(RandomM.Float0To1() > 0.5f ? Vector2.left : Vector2.right);
                HorizontalMovement.SetSpeed(RandomM.Float0To1() < walkBackwardsChance ? -moveSpeed : moveSpeed);

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
                HorizontalMovement.SetSpeed(0f);
                moveTimer = 0.0f;
            }
        }

        if (Attack.startAttackFlag)
        {
            HorizontalMovement.FaceDirection(Attack.attackDirection);
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

    private void UpdateAnimation()
    {
        if (Attack.startAttackFlag)
        {
            Animation.SetAnimatorAttack(true);
        }
        else if (Attack.stopAttackFlag)
        {
            Animation.SetAnimatorAttack(false);
        }

        if (Health.startBlockFlag)
        {
            Animation.SetAnimatorBlock(true);
        }
        else if (Health.stopBlockFlag)
        {
            Animation.SetAnimatorBlock(false);
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
                    SetAttackState(AttackState.Attack);
                }

                break;
        }

        attackLoopTimer += Time.deltaTime;
    }

    #region Late Update
    private void LateUpdate()
    {
        EntityControllerL.CharacterEntityLateUpdate(Attack, Health);
        
        EntityControllerL.ClearHorizontalMovementUpdate(HorizontalMovement);

        EntityControllerL.CharacterEntityLateUpdateClear(Attack, Health);
    }
    #endregion
}
