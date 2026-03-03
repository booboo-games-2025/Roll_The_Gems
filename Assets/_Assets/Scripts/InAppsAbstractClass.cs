using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using Prime31;

public abstract class InAppsAbstractClass : MonoBehaviour, InAppsInterface
{
    public abstract void InitializeInApps();
    //public abstract void PurchaseProduct(int index);
    public abstract void PurchaseProduct(string productId);
    public abstract void RestorePurchases();
    public abstract void SetPrice(Text t1, Text t2, Text t3);
}