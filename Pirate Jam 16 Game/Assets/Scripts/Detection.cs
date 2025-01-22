using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ComponentData
    {
        public Component component;
        public List<Collider2D> colliders = new List<Collider2D>();

        public ComponentData(Component component, List<Collider2D> colliders = null)
        {
            this.component = component;

            if (colliders != null)
            {
                foreach(Collider2D collider in colliders)
                    this.colliders.Add(collider);
            }
        }
    }

public static class Detection
{
    public static List<ComponentData> DetectComponent<T>(Vector2 pos, float radius, LayerMask layerMask = new LayerMask()) where T : Component
    {
        if (layerMask.value == 0)
            layerMask = ~0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, radius, layerMask);

        List<ComponentData> dataList = new List<ComponentData>();
        List<T> addedComponents = new List<T>();

        T component;
        ComponentData data;

        for (int i = 0; i < colliders.Length; i++)
        {
            component = colliders[i].GetComponentInParent<T>();

            if (component == null)
                continue;

            int index = addedComponents.IndexOf(component);

            if (index == -1) // Not yet added
            {
                data = new ComponentData(component);
                data.colliders.Add(colliders[i]);

                dataList.Add(data);
                addedComponents.Add(component);
            }
            else
            {
                dataList[index].colliders.Add(colliders[i]);
            }
        }        

        return dataList;
    }
}
