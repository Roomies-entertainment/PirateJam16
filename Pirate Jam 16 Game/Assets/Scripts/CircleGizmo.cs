using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class CircleGizmo : MonoBehaviour
{
    public float GetRadius()
    {
        Debug.Log(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z));
        return Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
    }

    private void OnDrawGizmos()
    {
        if (Selection.Contains(gameObject))
            Gizmos.DrawWireSphere(transform.position, GetRadius());
    }
}
