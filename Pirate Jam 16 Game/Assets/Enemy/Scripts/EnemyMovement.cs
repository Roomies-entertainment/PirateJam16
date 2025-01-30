using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float moveDelay = 3.0f;
    [SerializeField] private float moveDuration = 0.8f;

    [Header("")]
    [SerializeField] private UnityEvent onStartMoving;
    [SerializeField] private UnityEvent onStopMoving;

    private float moveTimer;
    private float velocityX = 0.0f;

    private void FixedUpdate()
    {
        transform.position += Vector3.right * velocityX * Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (velocityX == 0)
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
        velocityX = Random.value > 0.5f ? moveSpeed : -moveSpeed;

        FaceDirection(Vector2.right * (velocityX > 0 ? 1f : -1f));

        onStartMoving.Invoke();
    }

    private void StopMoving()
    {
        velocityX = 0.0f;

        onStopMoving.Invoke();
    }

    public void FaceDirection(Vector2 direction)
    {
        transform.localScale = new Vector3(
            transform.localScale.x * (direction.x > 0 ? 1f : -1f),
            transform.localScale.y,
            transform.localScale.z);
    }
}
