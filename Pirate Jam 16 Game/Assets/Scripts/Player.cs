using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerPhysics))]

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f;

    private PlayerInput Input;
    private PlayerPhysics Physics;

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Physics = GetComponent<PlayerPhysics>();
    }

    private void FixedUpdate()
    {
        Physics.AddForce(new Vector2(Input.movementInput * moveSpeed, 0f));
        Physics.AddForce(Physics2D.gravity * Time.fixedDeltaTime);

        Physics.DoFixedUpdate();
    }

    private void Update()
    {
        Input.DoUpdate();
    }
}
