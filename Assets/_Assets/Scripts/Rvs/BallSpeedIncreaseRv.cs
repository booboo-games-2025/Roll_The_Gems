using UnityEngine;
using System;

public class BallSpeedIncreaseRv : RvBase
{
    public static Action OnActive;
    public static bool IsActive = false;

    protected override void OnEffectStart()
    {
        IsActive = true;
        OnActive?.Invoke();
    }

    protected override void OnEffectEnd()
    {
        IsActive = false;
        OnActive?.Invoke();
    }
}
