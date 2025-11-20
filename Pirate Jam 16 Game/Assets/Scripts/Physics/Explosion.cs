using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystemSpawn))]
[RequireComponent(typeof(RandomizedSound))]
public class Explosion : MonoBehaviour
{
    ParticleSystemSpawn ParticleSystemSpawn;
    RandomizedSound RandomizedSound;

    public float fieldOfImpact = 3.2f;
    
    
    private const float foiToDamage = 0.25f;

    public float forceClose = 12f;
    public float forceFar = 8f;

    public int damage = 1;

    [Header("")]
    public bool destroySelf = true;


    private void Awake()
    {
        ParticleSystemSpawn = GetComponent<ParticleSystemSpawn>();
        RandomizedSound = GetComponent<RandomizedSound>();
    }

    public void Explode()
    {
        var components = Detection.DetectComponentsInParents(
            transform.position, fieldOfImpact, default, typeof(Rigidbody2D), typeof(IProcessExplosion));

        foreach (var component in components[typeof(Rigidbody2D)])
        {
            Rigidbody2D rb = (Rigidbody2D)component.Key;
            Vector2 toObject = rb.transform.position - transform.position;
            Vector2 force = toObject.normalized * Mathf.Lerp(forceFar, forceClose, toObject.magnitude / fieldOfImpact);

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
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }
}
