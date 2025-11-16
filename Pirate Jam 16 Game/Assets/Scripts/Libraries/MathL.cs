using UnityEngine;

public static class MathL
{
    public const float DivideBy360Factor = 0.00277f;
    public const float TinyFloat = 0.0000000001f;
    public const int Invalid = int.MaxValue;
    public static bool IsValid(float value) { return !Mathf.Approximately(value, Invalid); }

    public static float ScaledDeltaTime(float multiplier = 70f)
    {
        return Time.deltaTime * multiplier;
    }

    public static float ConvertZ1ToZ1Z(float t)
    {
        return 1f - Mathf.Abs(Mathf.Clamp01(t) * 2f - 1f);
    }
}
