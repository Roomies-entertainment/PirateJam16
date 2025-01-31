using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    [SerializeField] private UnityEvent<float, DetectionData> onStart;

    private void Start()
    {
        onStart.Invoke((float) health / startingHealth, null);
    }

    protected override void OnDie()
    {
        if (deathSound != null)
            SoundManager.PlaySoundNonSpatial(deathSound);
            
        onDie.Invoke();
    }

    public void DifficultySet(int hp){
        startingHealth = hp;
        health = startingHealth;
    }
}
