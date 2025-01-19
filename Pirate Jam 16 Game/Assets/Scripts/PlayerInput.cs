using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float movementInput { get; private set; }
    public bool jump { get; private set; }

    public void DoUpdate()
    {
        movementInput = Input.GetAxis("Horizontal");
        jump = Input.GetButtonDown("Jump");
    }
}
