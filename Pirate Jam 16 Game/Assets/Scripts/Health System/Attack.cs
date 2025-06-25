using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Attack : MonoBehaviour
{
    [Header("")]
    [SerializeField] private bool damagePlayers = true;
    [SerializeField] private bool damageEnemies = true;
    [SerializeField] protected bool damageObjects = true;
    [SerializeField] protected bool hitInteractables = false;

    [Header("")]
    [SerializeField] protected CircleGizmo AttackCircle;
    public const int BaseDamage = 1;

    [Header("")]
    [Tooltip("Don't hit objects behind")]
    [SerializeField] protected bool behindCheck = true;
    [SerializeField] protected float behindCheckLeniance = -0.3f;

    [Header("")]
    [SerializeField] protected bool debug;

    [Header("")]
    [SerializeField] protected UnityEvent<Vector2> onStartAttack;
    [SerializeField] protected UnityEvent<GameObject> onHitObject;
    [SerializeField] protected UnityEvent<GameObject> onHitBlocked;
    [SerializeField] protected UnityEvent<GameObject> onMissObject;
    [SerializeField] protected UnityEvent onStopAttack;

    protected void Start() { } // Ensures component toggle in inspector

    protected List<System.Type> GetDetectableTypes()
    {
        List<System.Type> types = new();

        if (damageObjects)
            types.Add(typeof(ObjectHealth));

        if (hitInteractables)
            types.Add(typeof(Interactable));

        if (damagePlayers)
            types.Add(typeof(PlayerHealth));

        if (damageEnemies)
            types.Add(typeof(EnemyHealth));


        return types;
    }

    public virtual bool FindComponents()
    {
        var types = GetDetectableTypes().ToArray();
        var detectedTypes = Detection.DetectComponentsInParents(
            AttackCircle.transform.position, AttackCircle.GetRadius(),
            default,
            types);

        foundComps.Clear();

        foreach (var type in detectedTypes)
        {
            foreach (var detected in type.Value)
            {
                var comp = (Component)detected.Key;
                var colliders = detected.Value;

                if (CanHitObject(comp.gameObject))
                {
                    foundComps.Add(comp, colliders);
                }
            }
        }

        return foundComps.Count > 0;
    }

    protected abstract bool CanHitObject(GameObject obj);

    public bool startAttackFlag { get; protected set; }
    public bool attackFlag { get; protected set; }
    public bool stopAttackFlag { get; protected set; }

    public Vector2 attackDirection { get; protected set; }
    public void SetAttackDirection(Vector2 setTo) { attackDirection = setTo; }

    public Vector2 GetAttackDirection()
    {
        Vector2 toFirst = foundComps.Keys.First().transform.position - transform.position;

        toFirst.y = 0f;
        toFirst.Normalize();

        return toFirst;
    }

    protected Dictionary<Component, List<Collider2D>> foundComps = new();


    public void PerformAttack(int damage = BaseDamage)
    {
        if (!attackFlag)
        {
            OnStartAttack(attackDirection);
        }

        AttackAndInteract(damage);
    }
    protected abstract void AttackAndInteract(int damage);

    protected virtual bool AttackDirectionHit(Vector2 objPosition)
    {
        return
            !behindCheck || attackDirection.sqrMagnitude == 0 || Detection.DirectionCheck(attackDirection, transform.position, objPosition, false, behindCheckLeniance);
    }

    protected virtual void OnStartAttack(Vector2 direction)
    {
        startAttackFlag = true;
        attackFlag = true;

        onStartAttack?.Invoke(direction);
    }

    protected virtual void OnHitObject(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{this} hit {obj.name}");
        }

        onHitObject?.Invoke(obj);
    }

    protected virtual void OnHitBlocked(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{this} blocked by {obj.name}");
        }

        onHitBlocked?.Invoke(obj);
    }

    protected virtual void OnMissObject(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{this} missed {obj.name}");
        }

        onMissObject?.Invoke(obj);
    }

    protected virtual void OnInteractObject(GameObject obj)
    {
        if (debug)
        {
            Debug.Log($"{this} interacted with {obj.name}");
        }
    }

    public virtual void StopAttack()
    {
        if (!attackFlag)
        {
            if (debug)
            {
                Debug.Log($"{this} in StopAttack() - not attacking");
            }

            return;
        }

        if (debug)
        {
            Debug.Log($"{this} attack complete");
        }

        OnStopAttack();
    }


    protected virtual void OnStopAttack()
    {
        stopAttackFlag = true;
        attackFlag = false;

        onStopAttack?.Invoke();
    }

    private void LateUpdate()
    {
        startAttackFlag = false;
        stopAttackFlag = false;
    }
}
