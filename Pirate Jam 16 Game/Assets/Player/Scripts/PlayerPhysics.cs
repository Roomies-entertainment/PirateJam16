using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    //public Rigidbody2D rb { get; private set; }
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsMaterial2D physicsMaterial;

    [SerializeField] [Tooltip("How long explosions will affect player's physics")]
    public float explosionDuration = 1f;

    public float speedX { get; private set; }
    public float speedY { get; private set; }

    public void InitializeRigidbody()
    {
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = physicsMaterial;
    }

    public void SyncForces()
    {
        speedX = rb.velocity.x;
        speedY = rb.velocity.y;
    }
    /* 
        public void AddForce(Vector2 force)
        {
            force *= Time.fixedDeltaTime;

            speedX += force.x;
            speedY += force.y;
        }
     */
 
    public void AddHorizontalSpeed(float speed)
    {
        speedX += speed;
    }

    public void AddVerticalSpeed(float speed)
    {
        speedY += speed;
    }


    public void EnforceHorizontalSpeed(float enforced)
    {
        if (enforced > 0 && speedX < enforced ||
            enforced < 0 && speedX > enforced)
        {
            speedX = enforced;
        }
    }

    public void EnforceVerticalSpeed(float enforced)
    {
        if (enforced > 0 && speedY < enforced || enforced < 0 && speedY > enforced)
        {
            speedY = enforced;
        }
    }

    public void ClampVerticalSpeed(float min, float max)
    {
        speedY = Mathf.Clamp(speedY, min, max);
    }

    public void ScaleSpeed(float scale)
    {
        speedX *= scale;
        speedY *= scale;
    }

    public void SlideAlongSurface(Vector2 surfaceNormal)
    {
        Vector2 currentSpeed = new Vector2(speedX, speedY);
        Vector2 slidSpeed = currentSpeed;

        float normalDot = Vector2.Dot(slidSpeed, surfaceNormal);

        slidSpeed -= surfaceNormal * normalDot;
        slidSpeed += surfaceNormal * Mathf.Max(0f, normalDot);

        speedX = slidSpeed.x;
        speedY = slidSpeed.y;
    }

    public void MovePlayer()
    {
        rb.velocity = new Vector2(speedX, speedY);
    }
}
