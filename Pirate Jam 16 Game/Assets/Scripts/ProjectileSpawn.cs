using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public Transform spawnPoint;

    [Header("")]
    public Vector2 direction;

    public RandomValue speed;
    public RandomValue delay;
    public float lifetime = 10f;
    public bool loop;
    private float timer;

    [Header("")]
    public bool flipProjSpriteX;
    public Sprite projectileSprite;
    public Collider2D[] ignoreColliders;

    private void Update()
    {
        if (loop && timer >= delay.GetValue(true))
        {
            SpawnProjectile();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    public GameObject SpawnProjectile() { return SpawnProjectile(spawnPoint.position, direction, speed.GetValue(true)); }
    public GameObject SpawnProjectile(Vector2 position = new Vector2(), Vector2 direction = new Vector2(), float speed = -1f)
    {
        if (position.sqrMagnitude == 0) { position = spawnPoint.position; }
        if (direction.sqrMagnitude == 0){ position = direction; }
        if (speed < 0)                  { speed = this.speed.GetValue(true); }

        direction.Normalize();

        GameObject p = new GameObject($"{this} - Projectile");
        p.transform.position = position;
        p.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, -90f, 0f);

        p.AddComponent<DestroyObject>().delay = lifetime;

        SpriteRenderer sR = p.AddComponent<SpriteRenderer>();
        sR.sprite = projectileSprite;
        sR.flipX = flipProjSpriteX;

        GameObject cObj = new GameObject("{this} - Projectile - Collider");
        cObj.layer = CollisionM.projectileLayer;
        cObj.transform.SetParent(p.transform, false);

        CircleCollider2D c = cObj.AddComponent<CircleCollider2D>();
        c.radius = Mathf.Max(
            projectileSprite.bounds.extents.x * p.transform.lossyScale.x,
            projectileSprite.bounds.extents.y * p.transform.lossyScale.y,
            projectileSprite.bounds.extents.z * p.transform.lossyScale.z) * 0.5f;

        foreach (Collider2D ign_c in ignoreColliders)
        {
            Physics2D.IgnoreCollision(c, ign_c);
        }

        Physics2DMove m = p.AddComponent<Physics2DMove>();
        m.movement = direction * speed;
        m.allowGravity = true;

        Rigidbody2D rb = p.EnforceComponentInParent<Rigidbody2D>();
        rb.angularDrag = 0.4f;

        FaceDirection fD = p.AddComponent<FaceDirection>();
        fD.mode = FaceDirection.Mode.TravelDirection;
        //fD.angle = -90;

        return p;
    }
}