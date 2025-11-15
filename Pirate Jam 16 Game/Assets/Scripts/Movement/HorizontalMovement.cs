using UnityEngine;
using UnityEngine.Events;

public class HorizontalMovement : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer sprite;

    [Header("")]
    [SerializeField] private UnityEvent<Vector2> onStartMoving;
    [SerializeField] private UnityEvent<Vector2> onStopMoving;

    public bool startMovingFlag { get; private set; }
    public bool stopMovingFlag { get; private set; }

    public float currentSpeed { get; private set; }
    private Vector2 faceDirection = Vector2.right;
    public Vector2 moveDirection { get { return (faceDirection.x * currentSpeed) > 0 ? Vector2.right : Vector2.left; } }

    private void Start() { } // Ensures component toggle in inspector

    public void SetSpeed(float speed)
    {
        if (currentSpeed == 0 && speed != 0)
        {
            startMovingFlag = true;
            onStartMoving?.Invoke((faceDirection * currentSpeed).normalized);
        }
        else if (currentSpeed != 0 && speed == 0)
        {
            stopMovingFlag = true;
            onStopMoving?.Invoke((faceDirection * currentSpeed).normalized);
        }

        currentSpeed = speed;
    }

    public void ApplyMovement()
    {
        rigidbody.velocity = new Vector2(faceDirection.x * currentSpeed, rigidbody.velocity.y);
    }

    public void Move(Vector2 movement)
    {
        rigidbody.MovePosition(rigidbody.position + movement);
    }

    public void MoveAwayFromCurrentDirection()
    {
        currentSpeed *= -1f;
    }

    public void TurnAwayFromCurrentDirection()
    {
        if (currentSpeed < 0)
        {
            currentSpeed *= -1f;
        }
        else
        {
            faceDirection *= -1f;
        }
    }

    public void FaceDirection(Vector2 direction)
    {
        faceDirection = direction;

        sprite.transform.localScale = new Vector3(
            Mathf.Abs(sprite.transform.localScale.x) * (faceDirection.x > 0 ? 1f : -1f),
            sprite.transform.localScale.y,
            sprite.transform.localScale.z);
    }

    private void LateUpdate()
    {
        startMovingFlag = false;
        stopMovingFlag = false;
    }
}