using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class CircleGizmo : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;

    public float GetRadius()
    {
        return Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
    }

    public void SetRadius(float setTo)
    {
        if (transform.parent)
        {
            transform.localScale = new Vector3(
                setTo / transform.parent.lossyScale.x,
                setTo / transform.parent.lossyScale.y,
                setTo / transform.parent.lossyScale.z);
        }
        else
        {
            transform.localScale = Vector3.one * setTo;
        }
    }

    private void OnDrawGizmos()
    {
        Color colorStore = Gizmos.color;
        Gizmos.color = color;

        Transform parentSearch = transform;

        while (parentSearch != null)
        {
            if (Selection.Contains(parentSearch.gameObject))
            {
                Gizmos.DrawWireSphere(transform.position, GetRadius());
                break;
            }

            parentSearch = parentSearch.parent;
        }

        Gizmos.color = colorStore;
    }
}
