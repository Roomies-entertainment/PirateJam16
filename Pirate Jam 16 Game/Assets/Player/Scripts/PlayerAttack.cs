using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : Attack
{
    [Header("")]
    [Range(0, 1)] public float fallingThreshold = 0.24f;
    public int fallingExtraDamage = 1;

    [Header("")]
    public float AttackDuration = 0f;
    public float attackTimer {get; private set; } = 0.0f;

    public List<GameObject> hitObjects { get; private set; } = new List<GameObject>();

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void FindComponents(
        out List<DetectedComponent<Health>> healthComponents,
        out List<DetectedComponent<Interactable>> interactables)
    {
        var components = Detection.DetectComponentsInParents(
            AttackCircle.transform.position, AttackCircle.GetRadius(),
            ~(1 << CollisionM.playerLayer),
            typeof(EnemyHealth), typeof(ObjectHealth), typeof(Interactable));
            
        healthComponents = new List<DetectedComponent<Health>>();
        interactables = new List<DetectedComponent<Interactable>>();

        foreach (var c in components)
        {
            var health = c.Key as Health;
            var interactable = c.Key as Interactable;

            if (health && CanHitObject(health.gameObject))
            {
                healthComponents.Add(new DetectedComponent<Health>(health, c.Value));
            }
            else if (interactable && CanHitObject(interactable.gameObject))
            {
                interactables.Add(new DetectedComponent<Interactable>(interactable));
            }
        }
        
    }

    public void PerformInteractions(List<DetectedComponent<Interactable>> detectedInteractableComponents)
    {
        foreach (var c in detectedInteractableComponents)
        {
            var interactable = c.Component;

            interactable.Interact();

            OnHitObject(interactable.gameObject);
        }
    }

    private bool CanHitObject(GameObject obj)
    {
        if (hitObjects.Contains(obj))
            return false;
            
        if (!AttackDirectionHit(obj.transform.position))
            return false;
        
        return true;
    }

    private bool AttackDirectionHit(Vector2 objPosition)
    {
        return
            !directionChecking ||
            attackDirection.sqrMagnitude == 0 ||
            Vector2.Dot(
                (objPosition - new Vector2(transform.position.x, transform.position.y)).normalized,
                attackDirection.normalized) > -directionCheckDistance;
    }

    protected override void OnStartAttack(Vector2 direction)
    {
        base.OnStartAttack(direction);

        attackTimer = 0.0f;
    }

    protected override void OnHitObject(GameObject obj)
    {
        hitObjects.Add(obj);

        base.OnHitObject(obj);
    }

    protected override void OnHitBlocked(GameObject obj)
    {
        hitObjects.Add(obj);

        base.OnHitBlocked(obj);
    }

    protected override void OnStopAttack()
    {
        base.OnStopAttack();

        hitObjects.Clear();
    }
}
