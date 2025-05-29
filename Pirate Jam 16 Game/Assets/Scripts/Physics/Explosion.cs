using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float fieldofImpact;

    public float force;

    public void Explode()
    {
        Detection.DetectComponentInParents<Rigidbody2D>(transform.position, fieldofImpact, out var rbs);

        foreach (Rigidbody2D rb in rbs)
        {
            //print(rb);

            Vector2 direction = rb.transform.position - transform.position;

            rb.AddForce(direction * force, ForceMode2D.Impulse);

            foreach (IProcessExplosion pE in rb.GetComponentsInChildren<IProcessExplosion>())
                pE.ProcessExplosion(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldofImpact);
    }
}
