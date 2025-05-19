using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbMovement : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 1f;

    private float climbTimer;
    public void Climb(float duration)
    {
        climbTimer = duration;
    }
    
    private void FixedUpdate()
    {
        if (climbTimer > 0)
        {
            PerformClimbing();
        }

        climbTimer -= Time.fixedDeltaTime;
    }

    private void PerformClimbing()
    {
        transform.Translate(Vector2.up * (climbSpeed * Time.fixedDeltaTime));
    }
}
