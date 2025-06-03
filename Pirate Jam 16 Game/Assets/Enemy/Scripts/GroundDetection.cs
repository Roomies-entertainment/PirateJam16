using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public OverlapCheck GroundCheck;
    public LROverlapChecks StepChecks;
    public LROverlapChecks StepClearanceChecks;
    public LROverlapChecks EdgeChecks;

    [Header("Sets the checks in Detection Components to these - won't allow manual update")]
    public FlagUpdateMode detectionUpdateMode = new FlagUpdateMode(
        Enums.UpdateMode.FixedUpdate, Enums.UpdateMode.LateUpdate);

    private void OnValidate()
    {
        if (detectionUpdateMode.setMode == Enums.UpdateMode.Manual)
        {
            detectionUpdateMode.setMode = Enums.UpdateMode.FixedUpdate;
        }

        if (detectionUpdateMode.clearMode == Enums.UpdateMode.Manual)
        {
            detectionUpdateMode.clearMode = Enums.UpdateMode.LateUpdate;
        }
    }

    private void Awake()
    {
        InitCheckUpdateModes();
    }

    public void InitCheckUpdateModes()
    {
        GroundCheck.flagUpdateMode = detectionUpdateMode;
        StepChecks.flagUpdateMode = detectionUpdateMode;
        StepClearanceChecks.flagUpdateMode = detectionUpdateMode;
        EdgeChecks.flagUpdateMode = detectionUpdateMode;
    }

    private void Start() { } // Ensures component toggle in inspector
}
