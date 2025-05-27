using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float fieldofImpact;

    public float force;

    public void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldofImpact);

        foreach (Collider2D obj in objects)
        {
            Rigidbody2D rb = obj.GetComponentInParent<Rigidbody2D>();

            print(rb);

            if (rb == null)
                continue;

            Vector2 direction = obj.transform.position - transform.position;

            rb.AddForce(direction * force, ForceMode2D.Impulse);

            foreach(IProcessExplosion pE in rb.GetComponentsInChildren<IProcessExplosion>())
                pE.ProcessExplosion();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldofImpact);
    }
}
