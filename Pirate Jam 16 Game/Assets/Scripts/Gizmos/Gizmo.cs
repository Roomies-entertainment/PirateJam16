using UnityEngine;
using UnityEditor;

public abstract class Gizmo : MonoBehaviour
{
    [SerializeField] private int visibileSelectDepth = 1;

    protected bool IsSelected()
    {
        Transform parentSearch = transform;
        int i = 0;

        while (parentSearch != null && i < (visibileSelectDepth + 1))
        {
            if (Selection.Contains(parentSearch.gameObject))
            {
                return true;
            }

            parentSearch = parentSearch.parent;

            i++;
        }

        return false;
    }

    protected abstract void OnDrawGizmos();
}
