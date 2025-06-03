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
    [Tooltip("Don't hit objects behind")]
    [SerializeField] protected bool behindCheck = true;
    [Tooltip("Don't hit objects beneath")]
    [SerializeField] protected bool beneathDirCheck = true;
    [SerializeField] protected float behindCheckLeniance = -0.3f;
    [Range(-1, 1)]
    [SerializeField] protected float beneathDirCheckLeniance = 0.5f;

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

    protected Dictionary<Component, List<Collider2D>> foundComps = new();

    public bool FindComponents()
    {
        var components = Detection.DetectComponentsInParents(
            AttackCircle.transform.position, AttackCircle.GetRadius(),
            default,
            GetDetectableTypes().ToArray());

        foundComps.Clear();

        foreach (var c in components)
        {
            if (CanHitObject(c.Key.gameObject))
            {
                foundComps.Add(c.Key, c.Value);
            }
        }

        return foundComps.Count > 0;
    }

    protected virtual List<System.Type> GetDetectableTypes()
    {
        List<System.Type> types = new();

        if (damageObjects)
            types.Add(typeof(ObjectHealth));

        if (hitInteractables)
            types.Add(typeof(Interactable));

        return types;
    }

    protected virtual bool CanHitObject(GameObject obj)
    {
        if (!AttackDirectionHit(obj.transform.position))
            return false;
        
        return true;
    }

    public void AttackAndInteract(int damage = BaseDamage)
    {
        if (!attacking)
        {
            if (debug)
            {
                Debug.Log($"{gameObject.name} attacking with {damage} damage");
            }

            OnStartAttack(attackDirection);
        }

        foreach (var c in foundComps)
        {
            Health health = c.Key as Health;
            Interactable interactable = c.Key as Interactable;

            if (health)
            {
                var result = health.ProcessAttack(
                    damage, new DetectionData(health.transform.position, health, this, c.Value));

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

            if (interactable)
            {
                interactable.Interact();

                OnInteractObject(interactable.gameObject);
            }
        }
    }

    protected virtual void OnStartAttack(Vector2 direction)
    {
        attacking = true;

        onStartAttack?.Invoke(direction);
    }

    protected virtual void OnHitObject(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} hit {obj.name}");
        }

        onHitObject?.Invoke(obj);
    }

    protected virtual void OnHitBlocked(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} blocked by {obj.name}");
        }

        onHitBlocked?.Invoke(obj);
    }

    protected virtual void OnMissObject(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} missed {obj.name}");
        }

        onMissObject?.Invoke(obj);
    }

    protected virtual void OnInteractObject(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{gameObject.name} interacted with {obj.name}");
        }
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
            (!beneathDirCheck || Detection.DirectionCheck(Vector2.up, transform.position, objPosition, true, beneathDirCheckLeniance)) &&
            (!behindCheck || attackDirection.sqrMagnitude == 0 || Detection.DirectionCheck(attackDirection, transform.position, objPosition, false, behindCheckLeniance));
    }
}
