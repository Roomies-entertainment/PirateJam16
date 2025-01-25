using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected float attackRadius = 2f;
    [SerializeField] public const int BaseDamage = 1;

    [Header("")]
    [SerializeField] protected AudioClip attackSound;

    [Header("")]
    [SerializeField] protected UnityEvent onAttack;

    [Header("")]
    [SerializeField] protected UnityEvent onStopAttack;

    public bool attacking { get; private set; }

    public void PerformAttack(List<ComponentData> attackedObjects, Vector2 attackDirection, int damage = BaseDamage)
    {
        foreach (var data in attackedObjects)
        {
            var health = (Health) data.component;

            if ( Vector2.Dot((health.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                health.ApplyDamage(damage, data);
                OnAttack(health);
            }
        }

        OnPerformAttack();
    }

    protected virtual void OnAttack(Health attackedObj) { }

    protected virtual void OnPerformAttack()
    {
        if (attackSound != null)
            SoundManager.PlaySoundNonSpatial(attackSound);

        attacking = true;

        onAttack.Invoke();
    }

    public void StopAttack()
    {
        OnStopAttack();
    }

    protected virtual void OnStopAttack()
    {
        attacking = false;

        onStopAttack.Invoke();
    }
}
