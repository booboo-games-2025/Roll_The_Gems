using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class TwoxIncomeRv : RvBase
{
    public static Action<float, bool> OnActive;

    protected override void OnEffectStart()
    {
        OnActive?.Invoke(2f,true);
    }

    protected override void OnEffectEnd()
    {
        OnActive?.Invoke(1f/2f, false);
    }
}
