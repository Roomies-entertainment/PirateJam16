using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleGizmo))]
public class OverlapCheck : MonoBehaviour
{
    [Header("")]
    [SerializeField] private LayerMask includeLayers = ~0;

    [Header("")]
    [SerializeField] protected UnityEvent<OverlapCheck> onCheckEnter;
    [SerializeField] protected UnityEvent<OverlapCheck> onCheckStay;
    [SerializeField] protected UnityEvent<OverlapCheck> onCheckExit;

    public bool check { get; private set; }

    private CircleGizmo circle;

    private void Awake()
    {
        circle = GetComponent<CircleGizmo>();
    }

    private void FixedUpdate()
    {
        PerformCheck();
    }

    public void PerformCheck()
    {
        bool checkStore = check;

        check = Physics2D.OverlapCircle(transform.position, circle.GetRadius(), includeLayers);

        if (!checkStore && check)
        {
            onCheckEnter?.Invoke(this);
        }
        else if (checkStore && check)
        {
            onCheckStay?.Invoke(this);
        }
        else if (checkStore && !check)
        {
            onCheckExit?.Invoke(this);
        }
    }
}
