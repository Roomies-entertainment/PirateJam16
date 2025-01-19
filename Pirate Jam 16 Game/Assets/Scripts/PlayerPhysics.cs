using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsMaterial2D physicsMaterial;

    private float velocityX;
    private float velocityY;

    private void Start()
    {
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = physicsMaterial;
    }

    public void AddForce(Vector2 force)
    {
        velocityX += force.x;
        velocityY += force.y;
    }

    public void DoFixedUpdate()
    {
        rb.velocity = new Vector2(velocityX, velocityY);

        velocityX = 0f;
    }
}
