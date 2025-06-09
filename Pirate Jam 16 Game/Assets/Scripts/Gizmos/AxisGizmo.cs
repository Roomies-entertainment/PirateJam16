using UnityEngine;

public class AxisGizmo : Gizmo
{
    [SerializeField] private float axisLength = 1f;

    protected override void OnDrawGizmos()
    {
        if (!IsSelected())
        {
            return;
        }

        Debug.DrawRay(transform.position, transform.right * transform.lossyScale.x * axisLength, Color.red);
        Debug.DrawRay(transform.position, transform.up * transform.lossyScale.y * axisLength, Color.green);
        Debug.DrawRay(transform.position, transform.forward * transform.lossyScale.z * axisLength, Color.blue);
    }
}
