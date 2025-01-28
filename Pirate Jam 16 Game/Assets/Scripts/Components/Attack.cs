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
    [SerializeField] protected UnityEvent onPerformAttack;
    [SerializeField] protected UnityEvent<GameObject> onAttackObject;
    [SerializeField] protected UnityEvent onStopAttack;

    public bool attacking { get; private set; }

    public void AttackObjects(List<ComponentData> attackedObjects, Vector2 attackPoint, Vector2 attackDirection, int damage = BaseDamage)
    {
        foreach (var data in attackedObjects)
        {
            var health = (Health) data.Component;

            if ( Vector2.Dot((health.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                health.ApplyDamage(
                    damage,
                    new DetectionData(attackPoint, data, new ComponentData(this)));
                    
                OnAttackObject(health.gameObject);
            }
        }

        if (!attacking)
            OnPerformAttack();
    }

    protected virtual void OnAttackObject(GameObject attackedObj)
    {
        onAttackObject.Invoke(attackedObj);
    }

    protected virtual void OnPerformAttack()
    {
        if (attackSound != null)
            SoundManager.PlaySoundNonSpatial(attackSound);

        attacking = true;

        onPerformAttack.Invoke();
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
