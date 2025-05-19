using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class HorizontalMovement : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer sprite;

    [Header("")]
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float moveDelay = 3.0f;
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField][Range(0, 1)] private float walkBackwardsChance = 0.3f;

    [Header("")]
    [SerializeField] private UnityEvent<Vector2> onStartMoving;
    [SerializeField] private UnityEvent<Vector2> onStopMoving;

    private float moveTimer;
    private Vector2 faceDirection = Vector2.right;
    private float currentSpeed = 0.0f;

    private void FixedUpdate()
    {
        MoveHorizontally();
    }

    private void MoveHorizontally()
    {
        rigidbody.velocity = new Vector2(faceDirection.x * currentSpeed, rigidbody.velocity.y);
    }

    private void Update()
    {
        if (currentSpeed == 0)
        {
            if (moveTimer > moveDelay)
            {
                StartMoving();

                moveTimer = 0.0f;
            }
        }
        else
        {
            if (moveTimer > moveDuration)
            {
                StopMoving();

                moveTimer = 0.0f;
            }
        }

        moveTimer += Time.deltaTime;
    }

    private void StartMoving()
    {
        currentSpeed = moveSpeed;

        if (Random.value < walkBackwardsChance)
        {
            currentSpeed *= -1f;
        }

        FaceDirection(Vector2.right * (Random.value > 0.5f ? 1 : -1));

        onStartMoving?.Invoke((faceDirection * currentSpeed).normalized);
    }

    private void StopMoving()
    {
        onStopMoving?.Invoke((faceDirection * currentSpeed).normalized);

        currentSpeed = 0.0f;
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
}
