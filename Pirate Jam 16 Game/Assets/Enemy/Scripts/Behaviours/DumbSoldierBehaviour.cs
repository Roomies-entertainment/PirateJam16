using UnityEngine;

public class DumbSoldierBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Components;
    [SerializeField] private GroundedEnemyDetection DetectionComponents;

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
    [SerializeField] private Vector2 blockDelayRange = new Vector2(0.3f, 0.4f);
    [SerializeField] private Vector2 blockDurationRange = new Vector2(1f, 1.5f);

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
        switch (setTo)
        {
            case AttackState.Attack:

                RandomizeAttackDelay();

                break;

            case AttackState.Attacking:

                RandomizeAttackDuration();
                Attack.FindComponents(out var healthComponents, out var interactables);

                Debug.Log(healthComponents);

                if (healthComponents.Count > 0)
                {
                    Attack.SetAttackDirection(Attack.GetAttackDirection(healthComponents));

                    Attack.PerformAttack(healthComponents);

                    transform.position += new Vector3(Attack.attackDirection.x, Attack.attackDirection.y, 0f) * 0.25f;
                }
                else if (interactables.Count > 0)
                {
                    Attack.SetAttackDirection(Attack.GetAttackDirection(interactables));

                    Attack.PerformInteractions(interactables);
                }

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
        Debug.Log(attackState);
    }

    private void FixedUpdate()
    {
        if (HorizontalMovement.enabled)
            FixedUpdateHorizontalMovement();

        if (JumpMovement.enabled)
            FixedUpdateJumpMovement();

        if (Attack.enabled)
            FixedUpdateAttack();

        if (Health.enabled)
            FixedUpdateHealth();

        if (Animation.enabled)
            FixedUpdateAnimation();

        if (Particles.enabled)
            FixedUpdateParticles();
    }

    private void FixedUpdateHorizontalMovement()
    {
        HorizontalMovement.MoveHorizontally();
    }

    private void FixedUpdateJumpMovement() { }
    private void FixedUpdateAttack() { }
    private void FixedUpdateHealth() { }
    private void FixedUpdateAnimation() { }
    private void FixedUpdateParticles() { }

    private void Update()
    {
        if (HorizontalMovement.enabled)
            UpdateHorizontalMovement();

        if (JumpMovement.enabled)
            UpdateJumpMovement();

        if (Attack.enabled)
            UpdateAttack();

        if (Health.enabled)
            UpdateHealth();

        if (Animation.enabled)
            UpdateAnimation();

        if (Particles.enabled)
            UpdateParticles();

        UpdateStepChecks();

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
            if (moveTimer > moveDuration)
            {
                HorizontalMovement.StopMoving();

                moveTimer = 0.0f;
            }
        }

        moveTimer += Time.deltaTime;
    }

    private void UpdateJumpMovement()
    {
        if (DetectionComponents.GroundCheck.check &&
            DetectionComponents.StepChecks.enterFlag &&
            (DetectionComponents.StepClearanceChecks.checkCount == 0) &&
            HorizontalMovement.currentSpeed != 0)
        {
            JumpMovement.Jump();
        }
    }

    private void UpdateAttack()
    {
        Debug.Log(attackLoopTimer);
        Debug.Log(attackState);

        switch (attackState)
        {
            case AttackState.Attack:

                if (attackLoopTimer > attackDelay)
                    SetAttackState(AttackState.Attacking);

                break;

            case AttackState.Attacking:

                if (attackLoopTimer > attackDuration)
                {
                    Attack.StopAttack();

                    if (DetectionComponents.PlayerCheck.check)
                        SetAttackState(AttackState.Block);
                    else
                        SetAttackState(AttackState.Attack);
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
                Health.StartBlocking();
            }
            else if (Health.blocking && attackStateChange[1] != AttackState.Blocking)
            {
                Health.StopBlocking();
            }
        }
    }

    private void UpdateAnimation()
    {

    }

    private void UpdateParticles()
    {

    }

    private void UpdateStepChecks()
    {
        if (HorizontalMovement.startMovingFlag)
        {
            Vector2 moveDir = HorizontalMovement.moveDirection;

            DetectionComponents.StepChecks.SetChecks(moveDir);
            DetectionComponents.StepClearanceChecks.SetChecks(moveDir);
        }

    }
}
