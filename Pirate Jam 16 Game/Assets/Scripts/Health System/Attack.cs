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

    protected Vector2 attackDirection;
    public void SetAttackDirection(Vector2 setTo) { attackDirection = setTo; }

    [Header("")]
    [SerializeField] protected bool debug;

    public bool attacking { get; private set; }

    public void PerformAttack(List<DetectedComponent<Health>> detectedHealthComponents, int damage = BaseDamage)
    {
        if (!attacking)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} attacking with {damage} damage");
            }

            OnStartAttack(attackDirection);
        }

        foreach (var detectedHC in detectedHealthComponents)
        {
            var health = detectedHC.Component;
            var result = health.ProcessAttack(
                damage, new DetectionData<Health, Attack>(health.transform.position, detectedHC, new DetectedComponent<Attack>(this)));
            
            switch(result)
            {
                case Health.AttackResult.Hit:
                    OnHitObject(health.gameObject); break;

                case Health.AttackResult.Miss:
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
