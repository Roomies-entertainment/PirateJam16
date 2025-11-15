using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    [SerializeField] [Tooltip("Sets direction")] private float angle;
    public Vector2 direction;

    [Space]
    public RandomValue speed;
    public RandomValue delay;
    public bool maintainSpeed;
    public bool loop;

    [Space]
    public float mass = 0.01f;
    public float startLifetime = 10f;
    public float collisionLifetime = 1f;
    private float timer;

    [Space]
    public Transform spawnPoint;
    public Sprite projectileSprite;
    public bool flipProjSpriteX;
    /* 
    [Space]
    public Collider2D[] ignoreColliders;
    */
    private void OnValidate()
    {
        if (Application.isEditor)
        {
            direction = Vector2L.FromAngle(angle);
        }
    }

    private void Update()
    {
        if (loop && timer >= delay.GetValue(true))
        {
            Spawn();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    
    // This ones just for events in the inspector
    public GameObject Spawn()
    {
        return Spawn(
            spawnPoint.position, direction, speed.GetValue(true), maintainSpeed,
            startLifetime, collisionLifetime, mass);
    }

    // This ones just for optional params
    public GameObject Spawn(
        float posX = Mathf.NegativeInfinity, float posY = Mathf.NegativeInfinity,
        Vector2 direction = new Vector2(),
        float speed = -1f, int maintainSpeed = -1,
        float startLifetime = -1f, float collisionLifetime = -1f, float mass = -1f)
    {
        if (posX == Mathf.NegativeInfinity) { posX = spawnPoint.position.x; }
        if (posY == Mathf.NegativeInfinity) { posY = spawnPoint.position.y; }
        if (direction.sqrMagnitude == 0)    { direction = this.direction; }
        if (speed < 0)                      { speed = this.speed.GetValue(true); }
        if (maintainSpeed < 0)              { maintainSpeed = this.maintainSpeed ? 1 : 0; }
        if (startLifetime < 0f)             { startLifetime = this.startLifetime; }
        if (collisionLifetime < 0f)         { collisionLifetime = this.collisionLifetime; }
        if (mass < 0f)                      { mass = this.mass; }

        return Spawn(new Vector2(posX, posY), direction, speed, maintainSpeed == 1, startLifetime, collisionLifetime, mass);
    }
    
    private GameObject Spawn(
        Vector2 position, Vector2 direction, float speed, bool maintainSpeed,
        float startLifetime = -1f, float collisionLifetime = -1f, float mass = -1f)
    {
        direction.Normalize();

        GameObject obj = new GameObject($"{this} - Projectile");
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, -90f, 0f);
        obj.layer = CollisionM.projectileLayer;

        SpriteRenderer sR = obj.EnforceComponent<SpriteRenderer>();
        sR.sprite = projectileSprite;
        sR.flipX = flipProjSpriteX;

        Collider2D c = obj.GetComponentInChildren<Collider2D>();
        if (c != null)
        {
            GameObject cObj = c.gameObject;
            DestroyImmediate(c);
            c = cObj.AddComponent<PolygonCollider2D>();
        }
        else
        {
            c = obj.AddComponent<PolygonCollider2D>();
        }

        Projectile p = obj.EnforceComponent<Projectile>();
        p.Initialize(speed, direction, maintainSpeed, startLifetime, collisionLifetime, mass);

        /* 
        foreach (Collider2D ign_c in ignoreColliders)
        {
            Physics2D.IgnoreCollision(c, ign_c);
        }
        */

        foreach (Collider2D ign_c in GetComponentsInChildren<Collider2D>())
        {
            Physics2D.IgnoreCollision(c, ign_c);
        }

        return obj;
    }
}