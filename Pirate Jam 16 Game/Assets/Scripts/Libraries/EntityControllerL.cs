using UnityEngine;

public static class EntityControllerL
{
    public static void EnemyCharacterLateUpdate(GameObject enemyCharacter,
        Health healthComponent, Attack attackComponent, HorizontalMovement hMovementComponent)
    {
        if (healthComponent.enabled) healthComponent.ProcessHealthEvents();

        hMovementComponent.ClearFlags();

        CharacterEntityLateUpdateClear(attackComponent, healthComponent);

        if (healthComponent.dead) enemyCharacter.SetActive(false);
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

    public static void CharacterEntityLateUpdateClear(Attack attackComponent, Health healthComponent)
    {
        attackComponent.ClearFlags();
        healthComponent.ClearUpdate();
    }
}