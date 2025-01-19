using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Collider2D handleCollider;
    [SerializeField] private Collider2D bladeCollider;

    [Header("")]
    [SerializeField] private bool debug = false;

    private void OnCollisionEnter(Collision collision)
    {
        var contact0 = collision.GetContact(0);
        
        if (contact0.thisCollider == handleCollider)
        {
            if (debug)
            {
                Debug.Log("Handle Collision");
            }
        }
        else if (contact0.thisCollider == bladeCollider)
        {
            if (debug)
            {
                Debug.Log("Blade Collision");
            }
        }
    }
}
