using UnityEngine;
using UnityEngine.Events;

using UpdateMode = Enums.UpdateMode;

public class LROverlapChecks : MonoBehaviour
{
    [Header(Constants.FlagUpdateMoveOverrideTTipStr)]
    [SerializeField] private OverlapCheck leftCheck;
    [SerializeField] private OverlapCheck rightCheck;

    [Tooltip(Constants.FlagUpdateModeTTStr)]
    public FlagUpdateMode flagUpdateMode;

    [Header("If min amount is above 1\nboth checks need to be in an onEnter state etc.")]
    [Space]
    [SerializeField][Range(1, 2)] private int minEnterAmount = 1;
    [SerializeField][Range(1, 2)] private int maxEnterAmount = 2;

    [Space]
    [SerializeField][Range(1, 2)] private int minStayAmount = 1;
    [SerializeField][Range(1, 2)] private int maxStayAmount = 2;

    [Space]
    [SerializeField][Range(1, 2)] private int minExitAmount = 1;
    [SerializeField][Range(1, 2)] private int maxExitAmount = 2;

    [Header("")]
    [SerializeField] private UnityEvent onCheckEnter;
    [SerializeField] private UnityEvent onCheckStay;
    [SerializeField] private UnityEvent onCheckExit;

    public bool enterFlag { get; private set; }
    public bool stayFlag { get; private set; }
    public bool exitFlag { get; private set; }

    public int checkCount { get { return (leftCheck.check ? 1 : 0) + (rightCheck.check ? 1 : 0); } }

    private void Awake()
    {
        leftCheck.flagUpdateMode.setMode = UpdateMode.Manual;
        rightCheck.flagUpdateMode.clearMode = UpdateMode.Manual;
    }

    private void FixedUpdate()
    {
        if (flagUpdateMode.clearMode == UpdateMode.FixedUpdate)
        {
            DoClearUpdate();
        }
        
        if (flagUpdateMode.setMode == UpdateMode.FixedUpdate)
        {
            DoSetUpdate();
        }
    }

    private void Update()
    {
        if (flagUpdateMode.clearMode == UpdateMode.Update)
        {
            DoClearUpdate();
        }

        if (flagUpdateMode.setMode == UpdateMode.Update)
        {
            DoSetUpdate();
        }
    }

    private void LateUpdate()
    {
        if (flagUpdateMode.clearMode == UpdateMode.LateUpdate)
        {
            DoClearUpdate();
        }

        if (flagUpdateMode.setMode == UpdateMode.LateUpdate)
        {
            DoSetUpdate();
        }
    }

    public void DoClearUpdate()
    {
        leftCheck.DoClearUpdate();
        rightCheck.DoClearUpdate();

        enterFlag = false;
        stayFlag = false;
        exitFlag = false;
    }

    public void DoSetUpdate()
    {
        leftCheck.DoSetUpdate();
        rightCheck.DoSetUpdate();

        enterFlag = CheckSetTo(
            true, leftCheck.enterFlag, rightCheck.enterFlag, minEnterAmount, maxEnterAmount);

        stayFlag = CheckSetTo(
            true, leftCheck.stayFlag, rightCheck.stayFlag, minStayAmount, maxStayAmount);

        exitFlag = CheckSetTo(
            true, leftCheck.exitFlag, rightCheck.exitFlag, minExitAmount, maxExitAmount);

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

    private bool CheckSetTo(bool setTo, bool leftFlag, bool rightFlag, int min, int max)
    {
        int amount = (leftFlag == setTo ? 1 : 0) + (rightFlag == setTo ? 1 : 0);

        return amount >= min && amount <= max;
    }

    public void SetCheckDir(Vector2 direction)
    {
        bool left = direction.x < 0;

        leftCheck.gameObject.SetActive(left);
        rightCheck.gameObject.SetActive(!left);
    }
}
