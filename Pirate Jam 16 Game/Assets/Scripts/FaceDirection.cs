using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    [Tooltip("Sets direction")]
    public float angle;
    public Vector2 direction;

    [Header("")]
    public float speedCutoff = 0.1f;

    private TrackTransform TrackTransform;

    public enum Mode
    {
        Override,
        TravelDirection
    }
    public Mode mode;

    void OnValidate()
    {
        if (mode == Mode.TravelDirection)
        {
            TrackTransform = gameObject.EnforceComponent<TrackTransform>();
            TrackTransform.updateMode = Enums.UpdateMode.Manual;
        }

        if (Application.isEditor)
        {
            direction = Vector2L.FromAngle(angle);
        }
    }

    void Start()
    {
        if (Application.isPlaying)
        {
            OnValidate();
        }
    }

    void FixedUpdate()
    {
        switch (mode)
        {
            case Mode.TravelDirection:

                TrackTransform.Track();

                if (TrackTransform.offset.sqrMagnitude > speedCutoff)
                {
                    Face(Vector2.Angle(Vector2.up, TrackTransform.offset));
                }

                break;

            case Mode.Override:

                Face(angle);

                break;
        }
    }

    void Face(float angle)
    {
        transform.rotation = (
            Quaternion.LookRotation(Vector3.forward) *
            Quaternion.AngleAxis(-angle, Vector3.forward));
    }
}
