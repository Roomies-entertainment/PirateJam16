using UnityEngine;

[HideInInspector]
public class PlayerInput : MonoBehaviour
{
    public float movementInput { get; private set; }
    public float movementInputActive { get; private set; }

    private bool _jumpFlag;      public bool jumpFlag { get { return _jumpFlag; } } public void ClearJumpFlag() { _jumpFlag = false; }
    private bool _attackFlag;    public bool attackFlag { get { return _attackFlag; } } public void ClearAttackFlag() { _attackFlag = false; }
    private bool _blockFlag;    public bool blockFlag { get { return _blockFlag; } }

    [SerializeField] private const float JumpTimeout = 0.35f;

    private float jumpTimer;
    private float attackTimer;

    public void DoUpdate()
    {
        movementInput = Input.GetAxisRaw("Horizontal");
        
        if (movementInput != 0f)
            movementInputActive = movementInput;

        HandleTimedFlag(ref _jumpFlag, Input.GetButtonDown("Jump"), ref jumpTimer, JumpTimeout);
        HandleTimedFlag(ref _attackFlag, Input.GetButtonDown("Attack"), ref attackTimer);
        HandleHoldFlag(ref _blockFlag, Input.GetButton("Block"));

        UpdateTimers();
    }

    private void HandleTimedFlag(ref bool flag, bool input, ref float timer, float timeout = -1f)
    {
        if (!flag && input)
        {
            flag = true;
            timer = 0f;
        }
        
        if (timeout > 0f && flag && timer > timeout)
        {
            flag = false;
        }
    }

    private void HandleHoldFlag(ref bool flag, bool input)
    {
        flag = input;
    }

    private void UpdateTimers()
    {
        jumpTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
    }
}
