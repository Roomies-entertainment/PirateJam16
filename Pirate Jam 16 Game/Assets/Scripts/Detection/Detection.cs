using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionData<DetectedType, DetectorType>
{
    public Vector2 Point { get; private set; }
    public DetectedComponent<DetectedType> DetectedComponent { get; private set; }
    public DetectedComponent<DetectorType> DetectorComponent { get; private set; }
    

    public DetectionData(
        Vector2 point, DetectedComponent<DetectedType> detectedComponentData, DetectedComponent<DetectorType> detectorComponentData)
    {
        Point = point;
        DetectedComponent = detectedComponentData;
        DetectorComponent = detectorComponentData;
    }
}

public class DetectedComponent<T>
{
    public T Component { get; private set; }
    public readonly List<Collider2D> Colliders = new List<Collider2D>();

    public DetectedComponent(T component, List<Collider2D> colliders)
    {
        Component = component;

        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
                Colliders.Add(collider);
        }
    }

    public DetectedComponent(T component, params Collider2D[] colliders)
    {
        Component = component;

        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
                Colliders.Add(collider);
        }
    }
}

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

    public static void DetectComponentInParent<Type>(
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
}
