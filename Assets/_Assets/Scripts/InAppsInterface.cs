using UnityEngine.UI;

//using Prime31;
public interface InAppsInterface
{
    void InitializeInApps();
    //void PurchaseProduct(int index);
    void PurchaseProduct(string productId);
    void RestorePurchases();
    void SetPrice(Text t1, Text t2, Text t3);
}