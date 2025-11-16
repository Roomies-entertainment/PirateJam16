using UnityEngine;

public static class Physics2DL
{
    public static Collider2D[] OverlapCircleAll(
        Vector2 pos, float radius, bool hitTriggers, LayerMask layerMask = default)
    {
        if (layerMask.value == 0)
            layerMask = ~0;
            
        bool triggerStore = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = hitTriggers;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layerMask);

        Physics2D.queriesHitTriggers = triggerStore;

        return colliders;
    }
}