using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected bool directionChecking;

    [Header("")]
    [SerializeField] protected float attackRadius = 2f;
    public const int BaseDamage = 1;

    [Header("")]
    [SerializeField] protected UnityEvent<Vector2> onPerformAttack;
    [SerializeField] protected UnityEvent<GameObject> onHitObject;
    [SerializeField] protected UnityEvent<GameObject> onMissObject;
    [SerializeField] protected UnityEvent onStopAttack;

    public bool attacking { get; private set; }

    public void AttackObjects(List<DetectedComponent> detectedHealthComponents, Vector2 attackPoint, Vector2 attackDirection, int damage = BaseDamage)
    {
        foreach (DetectedComponent detectedHC in detectedHealthComponents)
        {
            var health = (Health) detectedHC.Component;

            if (!directionChecking || Vector2.Dot((health.transform.position - transform.position).normalized, attackDirection.normalized) > 0f)
            {
                var result = health.ApplyDamage(damage, new DetectionData(attackPoint, detectedHC, new DetectedComponent(this)));
                
                switch(result)
                {
                    case Health.DamageResult.Hit:
                        onHitObject?.Invoke(health.gameObject); break;

                    case Health.DamageResult.Miss:
                        onMissObject?.Invoke(health.gameObject); break;
                }
            }
        }

        if (!attacking)
            OnPerformAttack(attackDirection);
    }

    protected virtual void OnAttackObject(GameObject attackedObj)
    {
        onHitObject?.Invoke(attackedObj);
    }

    protected virtual void OnPerformAttack(Vector2 direction)
    {
        attacking = true;

        onPerformAttack?.Invoke(direction);
    }

    public void StopAttack()
    {
        OnStopAttack();
    }

    protected virtual void OnStopAttack()
    {
        attacking = false;

        onStopAttack?.Invoke();
    }
}
