using UnityEngine;
using UnityEngine.InputSystem;

[HideInInspector]
public class PlayerInputs : MonoBehaviour
{
    public float horizontalInput { get; private set; }
    public float movementInputActive { get; private set; }

    public float verticalInput { get; private set; }

    private bool _jumpFlag;
    public bool jumpFlag { get { return _jumpFlag; } }

    private bool _attackFlag;
    public bool attackFlag { get { return _attackFlag; } }

    private bool _blockFlag;
    public bool blockFlag { get { return _blockFlag; } }

    public void OnMove(InputValue inputValue)
    {
        Vector2 value = inputValue.Get<Vector2>();

        horizontalInput = value.x;
        verticalInput = value.y;

        if (horizontalInput != 0f)
            movementInputActive = horizontalInput;
    }

    public void OnJump(InputValue inputValue)
    {
        HandleHoldFlag(ref _jumpFlag, inputValue.Get<float>() == 1f);
    }

    public void OnAttack(InputValue inputValue)
    {
        HandleHoldFlag(ref _attackFlag, inputValue.Get<float>() == 1f);
    }

    public void OnBlock(InputValue inputValue)
    {
        HandleHoldFlag(ref _blockFlag, inputValue.Get<float>() == 1f);
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
}
