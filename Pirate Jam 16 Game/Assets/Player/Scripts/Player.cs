using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerPhysics))]

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpSpeed = 9f;

    [Header("")]
    [SerializeField] private float upGravityScale = 2f;
    [SerializeField] private float downGravityScale = 1.7f;

    [Header("")]
    [SerializeField] private SurfaceDetector GroundDetector;

    private PlayerInput Input;
    private PlayerCollision Collision;
    private PlayerPhysics Physics;

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Collision = GetComponent<PlayerCollision>();
        Physics = GetComponent<PlayerPhysics>();
    }

    private void FixedUpdate()
    {
        bool onGround = GroundDetector.DetectSurface();

        if (onGround)
        {
            Physics.ApplyGroundMoveForce(Input.movementInput * moveSpeed);

            if (Input.jumpFlag)
            {
                Physics.ApplyJumpForce(jumpSpeed);
                Input.ClearJumpFlag();
            }
        }
        
        Physics.AddForce(Physics2D.gravity * Time.fixedDeltaTime * ( Physics.velocityY > 0 ? upGravityScale : downGravityScale ));

        Physics.DoFixedUpdate();
    }

    private void Update()
    {
        Input.DoUpdate();
    }
}
