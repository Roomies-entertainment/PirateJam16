using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class MeleeAttack : Attack
{
    protected override bool CanHitObject(GameObject obj)
    {
        if (!AttackDirectionHit(obj.transform.position))
            return false;

        return true;
    }

    protected override void AttackAndInteract(int damage = BaseDamage)
    {
        if (!attackFlag)
        {
            if (debug)
            {
                Debug.Log($"{this} attacking with {damage} damage");
            }
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
}
