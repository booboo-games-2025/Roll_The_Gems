using UnityEngine;

public class DurabilityInfiniteRv : RvBase
{
    public static bool IsActive = false;

    protected override void OnEffectStart()
    {
        IsActive = true;
    }

    protected override void OnEffectEnd()
    {
        IsActive = false;
    }
}
