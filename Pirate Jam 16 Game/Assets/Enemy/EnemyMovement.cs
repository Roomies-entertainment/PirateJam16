using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float moveDelay = 3.0f;
    [SerializeField] private float moveDuration = 0.8f;

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
    }

    private void StopMoving()
    {
        velocityX = 0.0f;
    }
}
