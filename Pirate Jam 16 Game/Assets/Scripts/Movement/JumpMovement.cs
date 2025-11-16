using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;

    [Header("")]
    public float force = 4.5f;
    public float cooldown = 0.3f;
    private float cooldownTimer;

    private void Start() { } // Ensures component toggle in inspector

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public void Jump(float forceOverride = MathL.Invalid)
    {
        if (cooldownTimer < cooldown)
        {
            return;
        }

        float force = MathL.IsValid(forceOverride) ? forceOverride : this.force;

        rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        cooldownTimer = 0f;
    }
}
