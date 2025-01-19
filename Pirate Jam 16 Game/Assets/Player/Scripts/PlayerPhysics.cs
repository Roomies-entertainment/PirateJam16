using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhysicsMaterial2D physicsMaterial;

    public float velocityX { get; private set; }
    public float velocityY { get; private set; }

    private void Start()
    {
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = physicsMaterial;
    }

    public void AddForce(Vector2 force)
    {
        velocityX += force.x;
        velocityY += force.y;
    }

    public void ApplyGroundMoveForce(float speed)
    {
        velocityX = speed;
    }

    public void ApplyJumpForce(float speed)
    {
        velocityY = speed;
    }

    public void DoFixedUpdate()
    {
        rb.velocity = new Vector2(velocityX, velocityY);
    }
}
