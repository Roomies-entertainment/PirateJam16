using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private bool maintainSpeed;
    [SerializeField] private float startLifetime = 10f;
    [SerializeField] private float collisionLifetime = 1.0f;

    [Space]
    public float mass = 0.01f;
    public int damage = 1;

    [Header("Controller for objects that already been hit")]
    [SerializeField] private bool phaseHitObjects = false;
    [SerializeField] private bool phaseDamagedObjects = true;

    private Rigidbody2D rb;
    private Collider2D col;
    private FaceDirection FaceDirection;
    private DestroyObject DestroyObjComp;

    private Vector2 lastVelocity;


    private void Awake()
    {
        rb = gameObject.EnforceComponentInParent<Rigidbody2D>();
        col = gameObject.EnforceComponentInChildren<PolygonCollider2D>();

        FaceDirection = gameObject.EnforceComponentInParent<FaceDirection>();

        DestroyObjComp = gameObject.EnforceComponentInParent<DestroyObject>();
        DestroyObjComp.enabled = false;
    }

    private void Start()
    {
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.angularDrag = 0.4f;
        rb.mass = mass;

        FaceDirection.mode = FaceDirection.Mode.TravelDirection;

        if (startLifetime > 0f)
        {
            DestroyObjComp.enabled = true;
            DestroyObjComp.timer = startLifetime;
        }

        SetForce();
    }

    public void Initialize(
        float speed, Vector2 direction, bool maintainSpeed = false,
        float startLifetime = -0.1f, float collisionLifetime = -0.1f, float mass = -0.1f)
    {

        this.speed = speed;
        this.direction = direction;
        this.maintainSpeed = maintainSpeed;
        this.startLifetime = startLifetime;
        this.collisionLifetime = collisionLifetime;
        this.mass = mass;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.rigidbody;
        Transform searchFrom = rb != null ? rb.transform : collision.collider.transform;

        var pPs = searchFrom.GetComponentsInChildren<IProcessProjectile>();

        if (-Vector3.Dot(lastVelocity, collision.GetContact(0).normal) > 11)
        {
            foreach (IProcessProjectile p in pPs)
                p.ProcessProjectile(this);
        }

        if (phaseHitObjects || phaseDamagedObjects && pPs.Length > 0)
            Physics2D.IgnoreCollision(col, collision.collider);

        if (collisionLifetime > 0f)
        {
            DestroyObjComp.enabled = true;
            DestroyObjComp.timer = Mathf.Min(collisionLifetime, DestroyObjComp.timer);
        }
    }

    private void FixedUpdate()
    {
        if (maintainSpeed)
        {
            SetForce();
        }

        lastVelocity = rb.velocity;
    }

    private void SetForce()
    {
        rb.velocity = direction * speed;
    }

    public void Deflect()
    {
        direction = RandomM.RandomDirection();
        direction = new Vector2(direction.x, 0.35f + direction.y * 0.75f);
        SetForce();
        rb.angularVelocity = 300f;

        FaceDirection.enabled = false;
    }
}
