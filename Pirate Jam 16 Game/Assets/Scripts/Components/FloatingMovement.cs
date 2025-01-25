using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    public float speed = 2f;

    void Update(){
        GetComponent<Rigidbody2D>().velocity = -transform.right * speed;
    }

    void Start(){
        transform.parent = null;
    }
}
