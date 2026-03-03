using UnityEngine;

public class DurabilityInfiniteRv : RvBase
{
    public static bool IsActive = false;

    protected override void OnEffectStart()
    {
        IsActive = true;
        GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.INFINITE_DURABILITY_RV);
    }

    protected override void OnEffectEnd()
    {
        IsActive = false;
    }
}
