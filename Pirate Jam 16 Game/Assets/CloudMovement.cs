using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2f;

    void Update(){
        GetComponent<Rigidbody2D>().velocity = -transform.right * speed;
    }
}
