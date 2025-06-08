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
    public FlagUpdateMode checkUpdateMode = new FlagUpdateMode(
        Enums.UpdateMode.FixedUpdate, Enums.UpdateMode.LateUpdate);

    private void OnValidate()
    {
        if (checkUpdateMode.setMode == Enums.UpdateMode.Manual)
        {
            checkUpdateMode.setMode = Enums.UpdateMode.FixedUpdate;
        }

        if (checkUpdateMode.clearMode == Enums.UpdateMode.Manual)
        {
            checkUpdateMode.clearMode = Enums.UpdateMode.LateUpdate;
        }
    }

    private void Awake()
    {
        InitCheckUpdateModes();
    }

    public void InitCheckUpdateModes()
    {
        GroundCheck.flagUpdateMode = checkUpdateMode;
        StepChecks.flagUpdateMode = checkUpdateMode;
        StepClearanceChecks.flagUpdateMode = checkUpdateMode;
        EdgeChecks.flagUpdateMode = checkUpdateMode;
    }

    private void Start() { } // Ensures component toggle in inspector
}
