using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class CircleGizmo : MonoBehaviour
{
    public float GetRadius()
    {
        return transform.lossyScale.x;
    }

    private void OnDrawGizmos()
    {
        if (Selection.Contains(gameObject))
            Gizmos.DrawWireSphere(transform.position, GetRadius());
    }
}
