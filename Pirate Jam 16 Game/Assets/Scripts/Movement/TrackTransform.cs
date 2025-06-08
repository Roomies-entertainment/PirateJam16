using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTransform : MonoBehaviour
{
    public Enums.UpdateMode updateMode;

    private Vector2 lastPos;
    public Vector2 offset { get; private set; }

    private void Awake()
    {
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (updateMode != Enums.UpdateMode.FixedUpdate)
        {
            return;
        }

        Track();
    }

    private void Update()
    {
        if (updateMode != Enums.UpdateMode.Update)
        {
            return;
        }

        Track();
    }

    private void LateUpdate()
    {
        if (updateMode != Enums.UpdateMode.LateUpdate)
        {
            return;
        }

        Track();
    }

    public void Track()
    {
        offset = new Vector2(
            transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        lastPos = new Vector2(
            transform.position.x, transform.position.y);
    }
}
