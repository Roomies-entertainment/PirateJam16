using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionData
{
    public Vector2 Point { get; private set; }
    public DetectedComponent DetectedComponent { get; private set; }
    public DetectedComponent DetectorComponent { get; private set; }
    

    public DetectionData(Vector2 point, DetectedComponent detectedComponentData, DetectedComponent detectorComponentData)
    {
        Point = point;
        DetectedComponent = detectedComponentData;
        DetectorComponent = detectorComponentData;
    }
}

public class DetectedComponent
{
    public Component Component { get; private set; }
    public readonly List<Collider2D> Colliders = new List<Collider2D>();

    public DetectedComponent(Component component, List<Collider2D> colliders = null)
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
    
    public static List<DetectedComponent> DetectComponent<T>(Vector2 pos, float radius, LayerMask layerMask = new LayerMask()) where T : Component
    {
        if (layerMask.value == 0)
            layerMask = ~0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layerMask);

        List<DetectedComponent> dataList = new List<DetectedComponent>();
        List<T> addedComponents = new List<T>();

        T component;
        DetectedComponent data;

        for (int i = 0; i < colliders.Length; i++)
        {
            component = colliders[i].GetComponentInParent<T>();

            if (component == null)
                continue;

            int index = addedComponents.IndexOf(component);

            if (index == -1) // Not yet added
            {
                data = new DetectedComponent(component);
                data.Colliders.Add(colliders[i]);

                dataList.Add(data);
                addedComponents.Add(component);
            }
            else
            {
                dataList[index].Colliders.Add(colliders[i]);
            }
        }        

        return dataList;
    }
}
