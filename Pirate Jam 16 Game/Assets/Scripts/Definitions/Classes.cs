using System;

[Serializable]
public class FlagUpdateMode
{
    public Enums.UpdateMode setMode;
    public Enums.UpdateMode clearMode;

    public FlagUpdateMode(Enums.UpdateMode setMode, Enums.UpdateMode clearMode)
    {
        this.setMode = setMode;
        this.clearMode = clearMode;
    }
}