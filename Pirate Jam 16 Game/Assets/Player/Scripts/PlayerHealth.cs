using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData<Health, Attack>> onStart;

    private void Start()
    {
        onStart?.Invoke((float) health / startingHealth, null);
    }

    protected override AttackResult ProcessDamageFlags(bool blocking, bool blockColliderHit, bool damageColliderHit)
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
