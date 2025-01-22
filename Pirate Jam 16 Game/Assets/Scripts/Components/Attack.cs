using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected float attackRadius = 2f;
    [SerializeField] protected int BaseDamage = 1;

    [Header("")]
    [SerializeField] protected AudioClip takeDamageSound;

    [Header("")]
    [SerializeField] protected UnityEvent onAttack;

    public void PerformAttack(List<Health> attackedObjects, Vector2 attackDirection)
    {
        foreach (var obj in attackedObjects)
        {
            if ( Vector2.Dot((obj.transform.position - transform.position).normalized, attackDirection.normalized) > 0f )
            {
                obj.TakeDamage(BaseDamage);
            }
        }

        OnAttack();
    }

    protected virtual void OnAttack()
    {
        if (takeDamageSound != null)
            SoundManager.PlaySoundNonSpatial(takeDamageSound);

        onAttack.Invoke();
    }
}
