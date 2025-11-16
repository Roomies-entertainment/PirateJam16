using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : CharacterHealth
{
    [HideInInspector] public bool deflectProjectiles;
    public float deflectionWindow = 0.3f;

    [Header("")]
    [SerializeField] private UnityEvent<float, DetectionData> onStart;
    [SerializeField] private UnityEvent<Projectile> onDeflect;

    protected new void Start()
    {
        base.Start();

        onStart?.Invoke((float) health / maxHealth, null);
    }

    public override void ProcessProjectile(Projectile p)
    {
        if (deflectProjectiles)
        {
            p.Deflect();
            onDeflect?.Invoke(p);

            return;
        }

        base.ProcessProjectile(p);
    }

    public void DifficultySet(int hp)
    {
        startingHealth = hp;
        health = startingHealth;
    }
}
