using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomM
{
    public static System.Random RandomSeedless = new();
    
    public static float Float0To1(System.Random random = null)
    {
        if (random == null)
            random = RandomSeedless;

        return (float) random.NextDouble();
    }
    public static float Range(float min, float max, System.Random random = null)
    {
        if (random == null)
            random = RandomSeedless;

        return Mathf.Lerp(min, max, (float) random.NextDouble());
    }
}
