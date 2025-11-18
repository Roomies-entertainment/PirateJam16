using UnityEngine;
using UnityEngine.Events;

using UpdateMode = Enums.UpdateMode;

[RequireComponent(typeof(CircleGizmo))]
public class OverlapCheck : MonoBehaviour
{
    [Tooltip(Constants.FlagUpdateModeTTStr)]
    public FlagUpdateMode flagUpdateMode;

    [Header("")]
    [SerializeField] private LayerMask includeLayers = ~0;

    [Header("")]
    [SerializeField] protected UnityEvent<OverlapCheck> onCheckEnter;
    [SerializeField] protected UnityEvent<OverlapCheck> onCheckStay;
    [SerializeField] protected UnityEvent<OverlapCheck> onCheckExit;

    public bool checkTrue { get; private set; }
    public bool enterFlag  { get; private set; }
    public bool stayFlag { get; private set; }
    public bool exitFlag { get; private set; }

    private CircleGizmo circle;

    private void Awake()
    {
        circle = GetComponent<CircleGizmo>();

        onCheckEnter.AddListener(GreenGizmo);
        onCheckExit.AddListener(RedGizmo);
    }
    private void Start() { RedGizmo(this); }
    public void GreenGizmo(OverlapCheck checkTrue) { circle.color = new Color(0, 0.43f, 0.28f, 1f); }
    public void RedGizmo(OverlapCheck checkTrue) { circle.color = Color.red * 0.7f; }


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
        enterFlag = false;
        stayFlag = false;
        exitFlag = false;
    }

    public void DoSetUpdate()
    {
        bool checkStore = checkTrue;

        checkTrue = Physics2D.OverlapCircle(transform.position, circle.GetRadius(), includeLayers);

        if (!checkStore && checkTrue)
        {
            enterFlag = true;
            onCheckEnter?.Invoke(this);
        }
        else if (checkStore && checkTrue)
        {
            stayFlag = true;
            onCheckStay?.Invoke(this);
        }
        else if (checkStore && !checkTrue)
        {
            exitFlag = true;
            onCheckExit?.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        onCheckEnter.RemoveListener(GreenGizmo);
        onCheckExit.RemoveListener(RedGizmo);
    }
}
