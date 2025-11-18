public static class EntityControllerL
{
    public static void CharacterEntityLateUpdate(Attack attackComponent, Health healthComponent)
    {
        if (healthComponent.enabled) ProcessHealthEvents(healthComponent);
    }

    public static void ProcessHealthEvents(Health healthComponent)
    {
        foreach (DamageEvent d in healthComponent.damageEvents)
            healthComponent.IncrementHealth(-d.amount, d.detectionData);

        foreach (DamageEvent h in healthComponent.healEvents)
            healthComponent.IncrementHealth(h.amount, h.detectionData);
        
        healthComponent.CheckIsDead();
    }

    public static void CharacterEntityLateUpdateClear(Attack attackComponent, Health healthComponent)
    {
        ClearAttackUpdate(attackComponent);
        ClearHealthUpdate(healthComponent);
    }
    
    public static void ClearAttackUpdate(Attack attackComponent)
    {
        attackComponent.ClearFlags();
    }

    public static void ClearHealthUpdate(Health healthComponent)
    {
        healthComponent.ClearHealthEvents();
        healthComponent.ClearFlags();
    }

    public static void ClearHorizontalMovementUpdate(HorizontalMovement horizontalMovement)
    {
        horizontalMovement.ClearFlags();
    }
}