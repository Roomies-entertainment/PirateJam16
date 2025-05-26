using System.Collections.Generic;

using UnityEngine;

public class PlayerAttack : Attack
{
    [Header("")]
    [SerializeField] private bool damagePlayers = false;
    [SerializeField] private bool damageEnemies = true;

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

    protected override List<System.Type> GetDetectableTypes()
    {
        List<System.Type> types = base.GetDetectableTypes();

        if (damagePlayers)
            types.Add(typeof(PlayerHealth));

        if (damageEnemies)
            types.Add(typeof(EnemyHealth));

        return types;
    }

    protected override bool CanHitObject(GameObject obj)
    {
        if (hitObjects.Contains(obj))
            return false;

        return base.CanHitObject(obj);
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
