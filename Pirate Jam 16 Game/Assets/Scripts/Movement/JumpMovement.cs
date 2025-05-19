using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;

    public void Jump(float force)
    {
        rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}
