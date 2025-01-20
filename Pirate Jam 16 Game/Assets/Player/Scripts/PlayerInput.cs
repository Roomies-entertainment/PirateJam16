using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float movementInput { get; private set; }
    public bool jumpFlag { get; private set; }
    public void ClearJumpFlag() { jumpFlag = false; }

    [SerializeField] private const float JumpTimeout = 0.35f;
    private float jumpFlagTimer;

    public void DoUpdate()
    {
        movementInput = Input.GetAxisRaw("Horizontal");

        if (!jumpFlag && Input.GetButtonDown("Jump"))
        {
            jumpFlag = true;
            jumpFlagTimer = 0f;
        }
        
        if (jumpFlag && jumpFlagTimer > JumpTimeout)
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
