using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [Header("")]
    [SerializeField] protected CircleGizmo AttackCircle;
    public const int BaseDamage = 1;

    [Header("")]
    [SerializeField] private UnityEvent<Vector2> onStartAttack;
    [SerializeField] private UnityEvent<GameObject> onHitObject;
    [SerializeField] private UnityEvent<GameObject> onMissObject;
    [SerializeField] private UnityEvent onStopAttack;

    [Header("")]
    [SerializeField] protected bool debug;

    public bool attacking { get; private set; }

    public void StartAttack(List<DetectedComponent> detectedHealthComponents, Vector2 attackDirection = new Vector2(), int damage = BaseDamage)
    {
        if (attacking)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} already attacking");
            }

            return;
        }

        if (debug)
        {
            Debug.Log($"{gameObject.name} attacking with {damage} damage");
        }

        OnStartAttack(attackDirection);

        foreach (DetectedComponent detectedHC in detectedHealthComponents)
        {
            var health = (Health) detectedHC.Component;
            var result = health.ApplyDamage(damage, new DetectionData(health.transform.position, detectedHC, new DetectedComponent(this)));
            
            switch(result)
            {
                case Health.DamageResult.Hit:
                    OnHitObject(health.gameObject); break;

                case Health.DamageResult.Miss:
                    OnMissObject(health.gameObject); break;
            }
        }
    }

    protected virtual void OnStartAttack(Vector2 direction)
    {
        attacking = true;

        onStartAttack?.Invoke(direction);
    }

    protected virtual void OnHitObject(GameObject attackedObj)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} hit {attackedObj.name}");
        }

        onHitObject?.Invoke(attackedObj);
    }

    protected virtual void OnMissObject(GameObject attackedObj)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} missed {attackedObj.name}");
        }

        onMissObject?.Invoke(attackedObj);
    }

    public virtual void StopAttack()
    {
        if (!attacking)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} in StopAttack() - not attacking");
            }

            return;
        }

        if (debug)
        {
            Debug.Log($"{gameObject.name} attack complete");
        }

        OnStopAttack();
    }

    protected virtual void OnStopAttack()
    {
        attacking = false;

        onStopAttack?.Invoke();
    }
}
