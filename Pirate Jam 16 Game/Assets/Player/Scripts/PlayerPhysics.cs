using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    //public Rigidbody2D rb { get; private set; }
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsMaterial2D physicsMaterial;

    public float speedX { get; private set; }
    public float speedY { get; private set; }

    public void InitializeRigidbody()
    {
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = physicsMaterial;
    }

    public void AddForce(Vector2 force)
    {
        force *= Time.fixedDeltaTime;
        
        speedX += force.x;
        speedY += force.y;
    }

    public void SetHorizontalSpeed(float speed)
    {
        speedX = speed;
    }

    public void SetVerticalSpeed(float speed)
    {
        speedY = speed;
    }

    public void ClampVerticalSpeed(float min, float max)
    {
        speedY = Mathf.Clamp(speedY, min, max);
    }

    public void MovePlayer()
    {
        rb.velocity = new Vector2(speedX, speedY);
    }
}
