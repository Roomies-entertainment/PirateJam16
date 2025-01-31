using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DMove : MonoBehaviour
{
    [SerializeField] private Vector2 movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = movement;
    }
}
