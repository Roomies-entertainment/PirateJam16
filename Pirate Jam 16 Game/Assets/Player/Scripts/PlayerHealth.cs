using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData> onStart;

    protected new void Start()
    {
        base.Start();
        
        onStart?.Invoke((float) health / maxHealth, null);
    }

    protected override AttackResult ProcessDamageFlags(
        bool blocking, bool blockColliderHit, bool damageColliderHit, DetectionData data)
    {
        if (blocking || blockColliderHit)
        {
            return AttackResult.Block;
        }

        if (damageColliderHit)
        {
            return AttackResult.Hit;
        }
        
        return AttackResult.Block;
    }

    public void DifficultySet(int hp){
        startingHealth = hp;
        health = startingHealth;
    }
}
