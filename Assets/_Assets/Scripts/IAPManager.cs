using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    public static IAPManager instance;
    private GameManager gameManager;
    [SerializeField] InAppManagerUnityIAP_Boombit inAppManagerUnityIAPNative;
    
    [SerializeField] UiButton starterPackBuyButton, premiumPackBuyButton;
    [SerializeField] private Sprite disabledSprite;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //UpdateUi();
    }

    /*public void PurchasePack(IAPPackType packType)
    {
        if (packType == IAPPackType.Coinpack1)
        {
            ProgressManager.instance.AddMoney(1000);
        }
        if (packType == IAPPackType.Coinpack2)
        {
            ProgressManager.instance.AddMoney(2500);
        }
        if (packType == IAPPackType.Coinpack3)
        {
            ProgressManager.instance.AddMoney(5000);
        }
        if (packType == IAPPackType.Starterpack)
        {
            ProgressManager.instance.AddMoney(5000);
            GameManager.instance.Activate2xIncomeBoostIAP();
            PlayerPrefs.SetInt(MyConstants.STARTER_PACK_PURCHASED,1);
        }
        if (packType == IAPPackType.Premiumpack)
        {
            ProgressManager.instance.AddMoney(10000);
            GameManager.instance.Activate2xIncomeBoostIAP();
            GameManager.instance.Activate2xPowerBoostIAP();
            PlayerPrefs.SetInt(MyConstants.PREMIUM_PACK_PURCHASED,1);
        }

        UpdateUi();
        //ProcessPurchase(packData);
    }

    void UpdateUi()
    {
        if (PlayerPrefs.GetInt(MyConstants.STARTER_PACK_PURCHASED,0) == 1)
        {
            starterPackBuyButton.Interactable = false;
            starterPackBuyButton.GetComponent<Image>().sprite = disabledSprite;
            starterPackBuyButton.GetComponent<TMP_Text>().text = "Purchased";
        }
        if (PlayerPrefs.GetInt(MyConstants.PREMIUM_PACK_PURCHASED,0) == 1)
        {
            premiumPackBuyButton.Interactable = false;
            premiumPackBuyButton.GetComponent<Image>().sprite = disabledSprite;
            premiumPackBuyButton.GetComponent<TMP_Text>().text = "Purchased";
        }
    }*/

    // private void ProcessPurchase(IAPPackData packData)
    // {
    //     OnPackPurchased?.Invoke(packData.PackType);
    // }
}

public enum IAPPackType
{
    Coinpack1,
    Coinpack2,
    Coinpack3,
    IncomePack,
    SpeedBundlePack,
    MegaUpgradePack
}