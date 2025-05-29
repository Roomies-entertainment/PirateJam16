using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Detection
{
    public enum CastType2D
    {
        Ray,
        Circle,
        Box,
        Capsule
    }

    public static Dictionary<Component, List<Collider2D>> DetectComponentsInParents(
        Vector2 pos, float radius, LayerMask layerMask = default, params System.Type[] types)
    {
        if (types.Length == 0)
        {
            throw new System.ArgumentException("At least one type must be provided.", nameof(types));
        }

        if (layerMask.value == 0)
            layerMask = ~0;

        bool triggerStore = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layerMask);

        Physics2D.queriesHitTriggers = triggerStore;

        var components = new Dictionary<Component, List<Collider2D>>();
        HashSet<GameObject> objectsAdded = new();

        for (int i = 0; i < colliders.Length; i++)
        {
            foreach (var T in types)
            {
                if (!T.IsSubclassOf(typeof(Component)))
                {
                    continue;
                }

                var component = colliders[i].GetComponentInParent(T);

                if (component == null)
                {
                    continue;
                }

                if (objectsAdded.Add(component.gameObject))
                {
                    components[component] = new();
                }

                components[component].Add(colliders[i]);
            }
        }

        return components;
    }

    public static void DetectComponentInParents<Type>(
        Vector2 pos, float radius, out List<Type> components, LayerMask layerMask = default) where Type : Component
    {
        if (layerMask.value == 0)
            layerMask = ~0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layerMask);

        components = new();
        HashSet<Type> addedComponents = new();

        for (int i = 0; i < colliders.Length; i++)
        {
            var component = colliders[i].GetComponentInParent<Type>();

            if (component == null || !addedComponents.Add(component))
            {
                continue;
            }

            components.Add(component);
        }
    }

    public static bool DirectionCheck(Vector2 directiom, Vector2 originPosition, Vector2 checkPosition, float distance = 0f)
    {
        return
            Vector2.Dot(
                (checkPosition - new Vector2(originPosition.x, originPosition.y)).normalized,
                directiom.normalized) > -distance;
    }
}

public class DetectionData
{
    public Vector2 Point { get; private set; }
    public Component DetectedComponent { get; private set; }
    public Component DetectorComponent { get; private set; }

    public readonly List<Collider2D> detectedColliders = new();
    public readonly List<Collider2D> detectorColliders = new();


    public DetectionData(
        Vector2 point,
        Component detectedComponent,
        Component detectorComponent,
        List<Collider2D> detectedColliders = null,
        List<Collider2D> detectorColliders = null)
    {
        Point = point;
        DetectedComponent = detectedComponent;
        DetectorComponent = detectorComponent;

        if (detectedColliders != null)
        {
            this.detectedColliders.AddRange(detectedColliders);
        }

        if (detectorColliders != null)
        {
            this.detectorColliders.AddRange(detectorColliders);
        }
    }
}