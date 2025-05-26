using UnityEngine;
using UnityEngine.InputSystem;

[HideInInspector]
public class PlayerInputs : MonoBehaviour
{
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

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        horizontalInput = value.x;
        verticalInput = value.y;

        if (horizontalInput != 0f)
            movementInputActive = horizontalInput;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        HandleTimedFlag(ref _jumpFlag, context.ReadValue<float>() == 1f, ref jumpTimer, JumpTimeout);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        HandleHoldFlag(ref _attackFlag, context.ReadValue<float>() == 1f);
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        HandleHoldFlag(ref _blockFlag, context.ReadValue<float>() == 1f);
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
