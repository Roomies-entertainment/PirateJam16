using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : CharacterHealth
{
    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData> onStart;

    protected new void Start()
    {
        base.Start();
        
        onStart?.Invoke((float) health / maxHealth, null);
    }

    public void DifficultySet(int hp){
        startingHealth = hp;
        health = startingHealth;
    }
}
