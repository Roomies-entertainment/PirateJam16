using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystemSpawn))]
[RequireComponent(typeof(RandomizedSound))]
public class Explosion : MonoBehaviour
{
    ParticleSystemSpawn ParticleSystemSpawn;
    RandomizedSound RandomizedSound;

    public float fieldofImpact;
    private const float foiToDamage = 0.5f;
    public int damage { get { return (int)Mathf.Round(fieldofImpact * foiToDamage); } }

    public float minForce = 8f;
    public float maxForce = 12f;

    [Header("")]
    public bool destroySelf = true;


    private void Awake()
    {
        ParticleSystemSpawn = GetComponent<ParticleSystemSpawn>();
        RandomizedSound = GetComponent<RandomizedSound>();
    }

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

        ParticleSystemSpawn.SpawnAndPlayParticles();
        RandomizedSound.PlaySound();

        if (destroySelf)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldofImpact);
    }
}
