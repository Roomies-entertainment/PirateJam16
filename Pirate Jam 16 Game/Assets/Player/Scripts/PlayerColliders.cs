using UnityEngine;

public class PlayerColliders : MonoBehaviour
{
    [SerializeField] private Transform Colliders;

    [Header("")]
    [SerializeField] private Transform UprightConfiguration;
    [SerializeField] private Transform FlatConfiguration;
    private Transform lastConfig;
    private Transform currentConfig;

    [Space]
    [SerializeField] private float configChangeDuration = 0.2f;
    private float configChangeTimer = 0.1f;
    private bool configChangeFlag = false;


    private void Start()
    {
        lastConfig = UprightConfiguration;
        currentConfig = UprightConfiguration;
        UpdateConfiguration(UprightConfiguration);

        configChangeTimer = configChangeDuration;
    }

    private void FixedUpdate()
    {
        if (configChangeFlag)
        {
            ApplyConfiguration();
        }

        configChangeTimer += Time.fixedDeltaTime;
    }

    private void ApplyConfiguration()
    {
        float t = configChangeTimer / configChangeDuration;

        Colliders.localPosition = Vector3.Lerp(lastConfig.localPosition, currentConfig.localPosition, t);
        Colliders.localRotation = Quaternion.Lerp(lastConfig.localRotation, currentConfig.localRotation, t);

        if (t > 1f)
        {
            configChangeFlag = false;
        }
    }

    public void SetFlatConfiguration()
    {
        UpdateConfiguration(FlatConfiguration);
    }

    public void SetUprightConfiguration()
    {
        UpdateConfiguration(UprightConfiguration);
    }

    private void UpdateConfiguration(Transform configuration)
    {
        lastConfig = currentConfig;
        currentConfig = configuration;

        configChangeTimer = 0f;
        configChangeFlag = true;
    }
}
