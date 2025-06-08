using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class KeyValueComponentThing
{
    public object component;
    public List<Collider2D> colliders;
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

    public static Dictionary<Type, Dictionary<object, List<Collider2D>>> DetectComponentsInParents(
        Vector2 pos, float radius, LayerMask layerMask = default, params Type[] types)
    {
        if (types.Length == 0)
        {
            throw new System.ArgumentException("At least one type must be provided.", nameof(types));
        }

        Collider2D[] colliders = Physics2DL.OverlapCircleAll(pos, radius, true, layerMask);

        Dictionary<
            Type, Dictionary<
                object, List<Collider2D>>> components = new();

        HashSet<Collider2D> colsAdded = new();

        for (int colI = 0; colI < colliders.Length; colI++)
        {
            Collider2D col = colliders[colI];

            foreach (Type type in types)
            {
                Component component = col.GetComponentInParent(type);
                if (component == null)
                    continue;

                if (!colsAdded.Add(col))
                    continue;

                if (!components.ContainsKey(type))
                    components[type] = new();

                if (!components[type].ContainsKey(component))
                    components[type][component] = new();

                components[type][component].Add(col);
            }
        }

        return components;
    }
    
    public static bool DirectionCheck(
        Vector2 direction, Vector2 originPosition, Vector2 checkPosition, bool normalizeOffset, float leniance = 0f)
    {
        Vector2 offset = (
            normalizeOffset ? (
                checkPosition - new Vector2(originPosition.x, originPosition.y)).normalized :
                checkPosition - new Vector2(originPosition.x, originPosition.y));

        return Vector2.Dot(offset, direction.normalized) > -leniance;
    }
}