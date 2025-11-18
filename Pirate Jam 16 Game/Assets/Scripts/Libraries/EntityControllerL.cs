using UnityEngine;

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

    public static void CharacterEntityHorizontalMovementUpdate(
        HorizontalMovement hMovementComponent, GroundDetection gDetectionComponent,
        ref float moveTimer, float moveDelay, float walkBackwardsChance, float moveSpeed, float moveDuration)
    {
        if (hMovementComponent.currentSpeed == 0)
        {
            if (moveTimer > moveDelay)
            {
                bool lr = RandomM.Float0To1() < 0.5f;

                hMovementComponent.FaceDirection(lr ? Vector2.left : Vector2.right);

                bool isGround = lr ? gDetectionComponent.EdgeChecks.leftCheck.checkTrue : gDetectionComponent.EdgeChecks.rightCheck.checkTrue;

                if (isGround)
                {
                    hMovementComponent.SetSpeed(RandomM.Float0To1() < walkBackwardsChance ? -moveSpeed : moveSpeed);
                }

                moveTimer = 0.0f;   
            }
        }
        else
        {
            if (gDetectionComponent.GroundCheck.checkTrue && moveTimer > moveDuration ||
                gDetectionComponent.GroundCheck.checkTrue && gDetectionComponent.EdgeChecks.exitFlag)
            {
                hMovementComponent.SetSpeed(0f);
                moveTimer = 0.0f;
            }
        }
    }

    public static void ClearHorizontalMovementUpdate(HorizontalMovement horizontalMovementComponent)
    {
        horizontalMovementComponent.ClearFlags();
    }
}