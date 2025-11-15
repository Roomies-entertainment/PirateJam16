using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected bool damageObjects = false;
    [SerializeField] protected bool hitInteractables = false;

    [Header("")]
    [SerializeField] protected CircleGizmo AttackCircle;
    public const int BaseDamage = 1;

    [Header("")]
    [SerializeField] protected bool directionChecking = true;
    [SerializeField] protected float directionCheckDistance = 1f;

    public Vector2 attackDirection { get; private set; }
    public void SetAttackDirection(Vector2 setTo) { attackDirection = setTo; }

    public bool attacking { get; private set; }

    [Header("")]
    [SerializeField] protected bool debug;

    [Header("")]
    [SerializeField] private UnityEvent<Vector2> onStartAttack;
    [SerializeField] private UnityEvent<GameObject> onHitObject;
    [SerializeField] private UnityEvent<GameObject> onHitBlocked;
    [SerializeField] private UnityEvent<GameObject> onMissObject;
    [SerializeField] private UnityEvent onStopAttack;

    protected virtual List<System.Type> GetDetectableTypes()
    {
        List<System.Type> types = new();

        if (damageObjects)
            types.Add(typeof(ObjectHealth));
            
        if (hitInteractables)
            types.Add(typeof(Interactable));

        return types;
    }


    public void FindComponents(
        out Dictionary<Health, List<Collider2D>> healthComponents,
        out Dictionary<Interactable, List<Collider2D>> interactables)
    {

        var components = Detection.DetectComponentsInParents(
            AttackCircle.transform.position, AttackCircle.GetRadius(),
            default,
            GetDetectableTypes().ToArray());

        healthComponents = new();
        interactables = new();

        foreach (var c in components)
        {
            var health = c.Key as Health;
            var interactable = c.Key as Interactable;

            if (health && CanHitObject(health.gameObject))
            {
                healthComponents[health] = new List<Collider2D>(c.Value);
                
            }
            else if (interactable && CanHitObject(interactable.gameObject))
            {
                interactables[interactable] = new List<Collider2D>(c.Value);
            }
        }
    }

    protected virtual bool CanHitObject(GameObject obj)
    {
        if (!AttackDirectionHit(obj.transform.position))
            return false;
        
        return true;
    }

    public void PerformAttack(Dictionary<Health, List<Collider2D>> healthComponents, int damage = BaseDamage)
    {
        if (!attacking)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} attacking with {damage} damage");
            }

            OnStartAttack(attackDirection);
        }

        foreach (var hc in healthComponents)
        {
            var health = hc.Key;
            var result = health.ProcessAttack(
                damage, new DetectionData(hc.Key.transform.position, hc.Key, this, hc.Value));

            switch (result)
            {
                case Health.AttackResult.Hit:
                    OnHitObject(health.gameObject); break;

                case Health.AttackResult.Block:
                    OnHitBlocked(health.gameObject); break;

                case Health.AttackResult.Miss:
                    OnMissObject(health.gameObject); break;
            }
        }
    }

    public void PerformInteractions(Dictionary<Interactable, List<Collider2D>> interactables)
    {
        foreach (var c in interactables)
        {
            var interactable = c.Key;

            interactable.Interact();

            OnHitObject(interactable.gameObject);
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

    protected virtual void OnHitBlocked(GameObject attackedObj)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} blocked by {attackedObj.name}");
        }

        onHitBlocked?.Invoke(attackedObj);
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
    
    protected bool AttackDirectionHit(Vector2 objPosition)
    {
        return
            !directionChecking ||
            attackDirection.sqrMagnitude == 0 ||
            Detection.DirectionCheck(attackDirection, transform.position, objPosition, directionCheckDistance);
    }
}
