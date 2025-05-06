using UnityEngine;
using UnityEngine.InputSystem;

[HideInInspector]
public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private bool enableBlocking;

    public float horizontalInput { get; private set; }
    public float movementInputActive { get; private set; }

    public float verticalInput { get; private set; }

    private bool _jumpFlag;
    public bool jumpFlag { get { return _jumpFlag; } }      public void ClearJumpFlag() { _jumpFlag = false; }

    private bool _attackFlag;
    public bool attackFlag { get { return _attackFlag; } }  public void ClearAttackFlag() { _attackFlag = false; }
    
    private bool _blockFlag;
    public bool blockFlag { get { return _blockFlag; } }    public void ClearBlockFlag() { _blockFlag = false;}

    [SerializeField] private const float JumpTimeout = 0.35f;

    private float jumpTimer;

    private void OnMove(InputValue inputValue)
    {
        Vector2 value = inputValue.Get<Vector2>();

        horizontalInput = value.x;
        verticalInput = value.y;

        if (horizontalInput != 0f)
            movementInputActive = horizontalInput;
    }

    private void OnJump(InputValue inputValue)
    {
        HandleTimedFlag(ref _jumpFlag, inputValue.isPressed, ref jumpTimer, JumpTimeout);
    }

    private void OnAttack(InputValue inputValue)
    {
        HandleHoldFlag(ref _attackFlag, inputValue.isPressed);
    }

    private void OnBlock(InputValue inputValue)
    {
        if (enableBlocking)
            HandleHoldFlag(ref _blockFlag, inputValue.isPressed);
        else if (_blockFlag)
            _blockFlag = false;
    }

    private void HandleHoldFlag(ref bool flag, bool isPressed)
    {
        if (!flag)
        {
            if (isPressed)
                flag = true;
        }
        else
        {
            if (!isPressed)
                flag = false;
        }
    }

    private void HandleTimedFlag(ref bool flag, bool isPressed, ref float timer, float timeout = float.PositiveInfinity)
    {
        if (!flag && isPressed)
        {
            flag = true;
            timer = 0f;
        }
        
        if (flag && timer > timeout)
        {
            flag = false;
        }
    }

    public void UpdateTimers()
    {
        jumpTimer += Time.deltaTime;
    }
}
