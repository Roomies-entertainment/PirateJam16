using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class LROverlapChecks : MonoBehaviour
{
    [SerializeField] private OverlapCheck leftCheck;
    [SerializeField] private OverlapCheck rightCheck;

    [Header("Events will be called depending on these values")]
    [SerializeField] private int enterAmountRequired = 0;
    [SerializeField] private int stayAmountRequired = 0;
    [SerializeField] private int exitAmountRequired = 0;

    [Header("")]
    [SerializeField] private UnityEvent onCheckEnter;
    [SerializeField] private UnityEvent onCheckStay;
    [SerializeField] private UnityEvent onCheckExit;

    private bool enterFlag;
    private bool stayFlag;
    private bool exitFlag;

    public void SetChecks(Vector2 direction)
    {
        bool left = direction.x < 0;

        leftCheck.gameObject.SetActive(left);
        rightCheck.gameObject.SetActive(!left);
    }

    private void FixedUpdate()
    {
        if (enterFlag && RequiredAmount(true, enterAmountRequired))
        {
            onCheckEnter?.Invoke();
        }

        if (stayFlag && RequiredAmount(true, stayAmountRequired))
        {
            onCheckStay?.Invoke();
        }

        if (exitFlag && RequiredAmount(false, exitAmountRequired))
        {
            onCheckExit?.Invoke();
        }

        ClearFlags();
    }

    private bool RequiredAmount(bool positive, int amountRequired)
    {
        if (amountRequired == 0 ||
            amountRequired == 1 && ( leftCheck.check == positive || rightCheck.check == positive ) ||
            amountRequired == 2 && leftCheck.check == positive && rightCheck.check == positive)
        {
            return true;
        }

        return false;
    }

    private void ClearFlags()
    {
        enterFlag = false;
        stayFlag = false;
        exitFlag = false;
    }

    public void OnCheckEnter(OverlapCheck check)
    {
        enterFlag = true;
    }

    public void OnCheckStay(OverlapCheck check)
    {
        stayFlag = true;
    }
    
    public void OnCheckExit(OverlapCheck check)
    {
        exitFlag = true;
    }
}
