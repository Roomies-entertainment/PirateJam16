using UnityEngine;

[HideInInspector]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool enableBlocking;

    public float movementInput { get; private set; }
    public float movementInputActive { get; private set; }

    private bool _jumpFlag;      public bool jumpFlag { get { return _jumpFlag; } } public void ClearJumpFlag() { _jumpFlag = false; }
    private bool _attackFlag;    public bool attackFlag { get { return _attackFlag; } } public void ClearAttackFlag() { _attackFlag = false; }
    private bool _blockFlag;    public bool blockFlag { get { return _blockFlag; } } public void ClearBlockFlag() { _blockFlag = false;}

    [SerializeField] private const float JumpTimeout = 0.35f;

    private float jumpTimer;

    public void DoUpdate()
    {
        movementInput = Input.GetAxisRaw("Horizontal");
        
        if (movementInput != 0f)
            movementInputActive = movementInput;

        HandleTimedFlag(ref _jumpFlag, "Jump", ref jumpTimer, JumpTimeout);
        HandleHoldFlag(ref _attackFlag, "Attack");

        if (enableBlocking)
            HandleHoldFlag(ref _blockFlag, "Block");
        else if (_blockFlag)
            _blockFlag = false;

        UpdateTimers();
    }

    private void HandleTimedFlag(ref bool flag, string inputName, ref float timer, float timeout = -1f)
    {
        if (!flag && Input.GetButtonDown(inputName))
        {
            flag = true;
            timer = 0f;
        }
        
        if (timeout > 0f && flag && timer > timeout)
        {
            flag = false;
        }
    }

    private void HandleHoldFlag(ref bool flag, string inputName)
    {
        if (!flag)
        {
            if (Input.GetButtonDown(inputName))
                flag = true;
        }
        else
        {
            if (!Input.GetButton(inputName))
                flag = false;
        }
    }

    private void UpdateTimers()
    {
        jumpTimer += Time.deltaTime;
    }
}
