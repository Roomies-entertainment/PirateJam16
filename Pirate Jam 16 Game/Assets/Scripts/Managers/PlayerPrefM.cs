using System;
using UnityEngine;

public static class PlayerPrefM
{
    public static float GetFloat(string data, float returnOnFail = MathL.Invalid) {

        if (PlayerPrefs.HasKey(data))
            return PlayerPrefs.GetFloat(data);

        return returnOnFail;
    }

    public static int GetInt(string data, int returnOnFail = MathL.Invalid)
    {
        if (PlayerPrefs.HasKey(data))
            return PlayerPrefs.GetInt(data);

        return returnOnFail;
    }

    public static bool GetBool(string data, bool returnOnFail = false)
    {
        if (PlayerPrefs.HasKey(data))
            return PlayerPrefs.GetInt(data) == 1 ? true : false;

        return returnOnFail;
    }
}