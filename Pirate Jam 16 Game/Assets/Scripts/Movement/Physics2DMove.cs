using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DMove : MonoBehaviour
{
    public Vector2 movement;
    public bool allowGravity;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.EnforceComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector3(
            movement.x,
            allowGravity ? rb.velocity.y + movement.y : movement.y,
            0f);
    }
}
