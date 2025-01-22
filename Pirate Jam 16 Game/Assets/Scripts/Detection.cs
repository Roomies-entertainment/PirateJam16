using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Detection
{
    public static List<T> DetectComponent<T>(Vector2 pos, float radius, LayerMask layerMask = new LayerMask())
    {
        if (layerMask == new LayerMask())
            layerMask = ~0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layerMask);
        List<T> components = new List<T>();

        T component;

        for (int i = 0; i < colliders.Length; i++)
        {
            component = colliders[i].GetComponentInParent<T>();

            if (component == null)
                continue;

            if (!components.Contains(component))
                components.Add(component);
        }        

        return components;
    }
}
