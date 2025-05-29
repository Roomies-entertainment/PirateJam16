using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float fieldofImpact;

    public float minForce = 8f;
    public float maxForce = 12f;

    public void Explode()
    {
        Detection.DetectComponentInParents<Rigidbody2D>(transform.position, fieldofImpact, out var rbs);

        foreach (Rigidbody2D rb in rbs)
        {
            //print(rb);

            Vector2 toObject = rb.transform.position - transform.position;
            Vector2 force = toObject.normalized * Mathf.Lerp(minForce, maxForce, toObject.magnitude / fieldofImpact);

            rb.AddForce(force, ForceMode2D.Impulse);

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
