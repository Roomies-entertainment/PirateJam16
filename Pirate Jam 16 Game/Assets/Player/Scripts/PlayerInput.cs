using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float movementInput { get; private set; }
    public bool jumpFlag { get; private set; }
    public void ClearJumpFlag() { jumpFlag = false; }

    private const float JumpFlagTimeout = 0.1f;
    private float jumpFlagTimer;

    public void DoUpdate()
    {
        movementInput = Input.GetAxis("Horizontal");

        if (!jumpFlag && Input.GetButtonDown("Jump"))
        {
            jumpFlag = true;
            jumpFlagTimer = 0f;
        }
        
        if (jumpFlag && jumpFlagTimer > JumpFlagTimeout)
        {
            jumpFlag = false;
        }

        UpdateTimers();
    }

    private void UpdateTimers()
    {
        jumpFlagTimer += Time.deltaTime;
    }
}
