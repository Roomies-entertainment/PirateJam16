using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDetection
{
    public static List<GameObject> DetectEnemies(Vector2 playerPos, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPos, radius, 1 << Collisions.enemyLayer);
        List<GameObject> enemies = new List<GameObject>();

        GameObject enemy;

        for (int i = 0; i < colliders.Length; i++)
        {
            enemy = colliders[i].gameObject;

            if (!enemies.Contains(enemy))
                enemies.Add(enemy);
        }        

        return enemies;
    }
}
