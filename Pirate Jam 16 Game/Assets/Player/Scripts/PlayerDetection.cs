using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDetection
{
    public static List<Enemy> DetectEnemies(Vector2 playerPos, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPos, radius, 1 << Collisions.enemyLayer);
        List<Enemy> enemies = new List<Enemy>();

        Enemy enemy;

        for (int i = 0; i < colliders.Length; i++)
        {
            enemy = colliders[i].GetComponent<Enemy>();

            if (enemy == null)
                continue;

            if (!enemies.Contains(enemy))
                enemies.Add(enemy);
        }        

        return enemies;
    }
}
