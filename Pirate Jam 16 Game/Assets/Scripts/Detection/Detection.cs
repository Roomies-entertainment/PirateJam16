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

    public DetectedComponent(T component, params Collider2D[] colliders)
    {
        Component = component;

        if (colliders != null)
        {
            foreach(Collider2D collider in colliders)
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
    
public static void DetectComponentsInParent(
    Vector2 pos, float radius, out Dictionary<Collider2D, Object> components,
    LayerMask layerMask = default, params System.Type[] types)
{
    if (types.Length == 0)
    {
        throw new System.ArgumentException("At least one type must be provided.", nameof(types));
    }

    if (layerMask.value == 0)
        layerMask = ~0;

    Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layerMask);

    components = new();
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

            if (component == null || !objectsAdded.Add(component.gameObject))
            {
                continue;
            }

            Debug.Log(component);

            components[colliders[i]] = component;
        }
    }
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
