using UnityEngine;

[ExecuteAlways]
public class CircleGizmo : Gizmo
{
    public Color color = Color.white;

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

    protected override void OnDrawGizmos()
    {
        if (!IsSelected())
        {
            return;
        }

        Color colorStore = Gizmos.color;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, GetRadius());
        Gizmos.color = colorStore;
    }
}
