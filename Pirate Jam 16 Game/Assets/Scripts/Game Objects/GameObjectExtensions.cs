using UnityEngine;

public static class GameObjectExtensions
{
    public static T EnforceComponent<T>(this GameObject obj) where T : Component
    {
        if (obj.TryGetComponent<T>(out var component))
        {
            return component;
        }

        return obj.AddComponent<T>();
    }

    public static T EnforceComponentInParent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponentInParent<T>();

        if (component != null)
        {
            return component;
        }

        return obj.AddComponent<T>();
    }

    public static T EnforceComponentInChildren<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponentInChildren<T>();

        if (component != null)
        {
            return component;
        }

        return obj.AddComponent<T>();
    }
}
