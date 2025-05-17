using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomM
{
    public static System.Random RandomSeedless { get; private set; } = new();

    public static float Float0To1(System.Random random = null)
    {
        if (random == null)
            random = RandomSeedless;

        return (float)random.NextDouble();
    }

    public static float Range(float min, float max, System.Random random = null)
    {
        if (random == null)
            random = RandomSeedless;

        return Mathf.Lerp(min, max, (float) random.NextDouble());
    }

    public static Vector2 RandomDirection(System.Random random = null)
    {
        if (random == null)
            random = RandomSeedless;

        float angle = (float) (random.NextDouble() * 2 * Mathf.PI);
        
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
