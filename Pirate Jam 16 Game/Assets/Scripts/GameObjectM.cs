using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectM
{
    public static GameObject FindGameObjectWithTag(Tags.TagType type)
    {
        var normalTag = GameObject.FindGameObjectWithTag(type.ToString());

        if (normalTag)
            return normalTag;

        var tagComponent = GameObject.FindObjectOfType<Tags>();

        if (tagComponent.CheckTag(type))
            return tagComponent.gameObject;

        return null;
    }

    public static List<GameObject> FindGameObjectsWithTag(Tags.TagType type)
    {
        var objects = new List<GameObject>();

        objects.AddRange(GameObject.FindGameObjectsWithTag(type.ToString()));

        var tagComponents = GameObject.FindObjectsOfType<Tags>();

        for (int i = 0; i < tagComponents.Length; i++)
        {
            if (tagComponents[i].CheckTag(type))
                objects.Add(tagComponents[i].gameObject);
        }

        return objects;
    }
}