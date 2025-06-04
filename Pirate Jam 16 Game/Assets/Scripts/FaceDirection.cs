using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    public float angle;
    [SerializeField] private Vector2 _directionDisplay;

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

        Vector3 d = Quaternion.AngleAxis(-angle, Vector3.forward) * Vector3.up;

        _directionDisplay = new Vector2(
            Mathf.Floor(d.x*10)/10,
            Mathf.Floor(d.y*10)/10
        );
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
