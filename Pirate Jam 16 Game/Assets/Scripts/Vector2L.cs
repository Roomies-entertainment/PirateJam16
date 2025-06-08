using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2L
{
    public static Vector2 FromAngle(float angle)
    {
        // Calculates it
        Vector3 d = Quaternion.AngleAxis(-angle, Vector3.forward) * Vector3.up;
        
        // Rounds it
        Vector2 direction = new Vector2(
            Mathf.Floor(d.x * 10) / 10,
            Mathf.Floor(d.y * 10) / 10
        );

        return direction;
    }
}
