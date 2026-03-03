using UnityEngine;
using System;

public class RingHealthHalfRv : RvBase
{
    public static Action OnActive;
    public static bool IsActive = false;

    protected override void OnEffectStart()
    {
        IsActive = true;
        OnActive?.Invoke();
        GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.TWOX_RING_DAMAGE_RV);
    }

    protected override void OnEffectEnd()
    {
        IsActive = false;
        //OnActive?.Invoke();
    }
}
