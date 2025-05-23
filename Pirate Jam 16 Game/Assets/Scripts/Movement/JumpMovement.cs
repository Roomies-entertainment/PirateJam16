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

    public void Jump(float forceOverride = float.NaN)
    {
        if (cooldownTimer < cooldown)
        {
            return;
        }

        float force = float.IsNaN(forceOverride) ? this.force : forceOverride;

        rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        cooldownTimer = 0f;
    }
}
