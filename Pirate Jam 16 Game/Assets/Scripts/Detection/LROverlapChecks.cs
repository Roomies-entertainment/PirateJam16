using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class LROverlapChecks : MonoBehaviour
{
    [SerializeField] private OverlapCheck leftCheck;
    [SerializeField] private OverlapCheck rightCheck;

    [Header("If min amount is above 1\nboth checks need to be in an onEnter state etc.")]
    [Space]
    [SerializeField] [Range(1, 2)] private int minEnterAmount = 1;
    [SerializeField] [Range(1, 2)] private int maxEnterAmount = 2;

    [Space]
    [SerializeField] [Range(1, 2)] private int minStayAmount = 1;
    [SerializeField] [Range(1, 2)] private int maxStayAmount = 2;

    [Space]
    [SerializeField] [Range(1, 2)] private int minExitAmount = 1;
    [SerializeField] [Range(1, 2)] private int maxExitAmount = 2;

    [Header("")]
    [SerializeField] private UnityEvent onCheckEnter;
    [SerializeField] private UnityEvent onCheckStay;
    [SerializeField] private UnityEvent onCheckExit;

    private bool enterFlagUV; public bool enterFlag { get; private set; }
    private bool stayFlagUV; public bool stayFlag { get; private set; }
    private bool exitFlagUV; public bool exitFlag { get; private set; }

    public int checkCount { get { return (leftCheck.check ? 1 : 0) + (rightCheck.check ? 1 : 0); } }

    public void OnCheckEnter(OverlapCheck check)
    {
        enterFlagUV = true;
    }

    public void OnCheckStay(OverlapCheck check)
    {
        stayFlagUV = true;
    }

    public void OnCheckExit(OverlapCheck check)
    {
        exitFlagUV = true;
    }

    private void FixedUpdate()
    {
        enterFlag = enterFlagUV && RequiredAmount(minEnterAmount, maxEnterAmount, true);
        stayFlag = stayFlagUV && RequiredAmount(minStayAmount, maxStayAmount, true);
        exitFlag = exitFlagUV && RequiredAmount(minExitAmount, maxExitAmount, false);

        enterFlagUV = false;
        stayFlagUV = false;
        exitFlagUV = false;

        if (enterFlag)
        {
            onCheckEnter?.Invoke();
        }

        if (stayFlag)
        {
            onCheckStay?.Invoke();
        }
        if (exitFlag)
        {

            onCheckExit?.Invoke();
        }
    }

    private bool RequiredAmount(int minAmount, int maxAmount, bool positive)
    {
        int amount = (leftCheck.check == positive ? 1 : 0) + (rightCheck.check == positive ? 1 : 0);

        return amount >= minAmount && amount <= maxAmount;
    }
    
    public void SetChecks(Vector2 direction)
    {
        bool left = direction.x < 0;

        leftCheck.gameObject.SetActive(left);
        rightCheck.gameObject.SetActive(!left);
    }
}
