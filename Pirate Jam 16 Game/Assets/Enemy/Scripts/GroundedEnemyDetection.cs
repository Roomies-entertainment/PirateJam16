using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemyDetection : MonoBehaviour
{
    public OverlapCheck PlayerCheck;
    public OverlapCheck GroundCheck;
    public LROverlapChecks StepChecks;
    public LROverlapChecks StepClearanceChecks;
}
