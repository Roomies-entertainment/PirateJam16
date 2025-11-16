using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnM : MonoBehaviour
{
    [Tooltip(   "Children are used as respawn points\n" +
                "If unassigned, will be set to anything tagged \"Respawn\" automatically")]
    [SerializeField] private GameObject respawnPointParent;

    private void OnValidate()
    {
        CheckRespawnParentReference();
    }

    private void CheckRespawnParentReference()
    {
        if (respawnPointParent == null)
        {
            respawnPointParent = GameObject.FindGameObjectWithTag("Respawn");
        }
    }

    public void RespawnObject(GameObject obj)
    {
        CheckRespawnParentReference();

        if (respawnPointParent == null)
        {
            Debug.Log("No object found with tag \"Respawn\"");

            return;
        }

        Transform parentT = respawnPointParent.transform;

        if (parentT.childCount == 0)
        {
            Debug.Log("No respawn points found");

            return;
        }

        Transform closest = transform;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < parentT.childCount; i++)
        {
            Transform child = parentT.GetChild(i);

            float distance = Vector3.Distance(child.position, obj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = child;
            }
        }

        obj.transform.position = closest.position;
        obj.SetActive(true);
    }
}
