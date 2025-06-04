using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionM
{
    public static int playerLayer = 6;
    public static int enemyLayer = 7;
    public static int projectileLayer = 9;

    public static bool IgnoreColliderInMatrix(GameObject obj, Collider2D col)
    {
        return Physics.GetIgnoreLayerCollision(obj.layer, col.gameObject.layer);
    }
}
