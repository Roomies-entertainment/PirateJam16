using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float walkBackwardsChance = 0.3f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float moveDelay = 3.0f;
    [SerializeField] private float moveDuration = 0.8f;

    [Header("")]
    [SerializeField] private UnityEvent onStartMoving;
    [SerializeField] private UnityEvent onStopMoving;

    private float moveTimer;
    private Vector2 faceDirection = Vector2.right;
    private float currentSpeed = 0.0f;

    private void FixedUpdate()
    {
        transform.Translate(faceDirection * (currentSpeed * Time.fixedDeltaTime));
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

        onStartMoving?.Invoke();
    }

    private void StopMoving()
    {
        currentSpeed = 0.0f;

        onStopMoving?.Invoke();
    }

    public void FaceDirection(Vector2 direction)
    {
        faceDirection = direction;

        transform.localScale = new Vector3(
            Mathf.Abs(transform.localScale.x) * (faceDirection.x > 0 ? 1f : -1f),
            transform.localScale.y,
            transform.localScale.z);
    }
}
