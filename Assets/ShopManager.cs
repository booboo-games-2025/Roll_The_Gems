using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    
    [Header("IncomeBundleUi")]
    [SerializeField] UiButton incomeBundleBuyButton;
    [SerializeField] private TMP_Text incomeBundleCostText;
    
    [Header("Speed&PowerBundleUi")]
    [SerializeField] UiButton speedAndPowerBundleBuyButton;
    [SerializeField] private TMP_Text speedAndPowerBundleCostText;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(MyConstants.INCOME_BUNDLE_PURCHASED, 0) == 1)
        {
            RvManager.instance.ClearRv(0);
            UpgradeManager.instance.SetIncomeMultiplier(1.5f, true);
        }
        
        if (PlayerPrefs.GetInt(MyConstants.INCOME_BUNDLE_PURCHASED, 0) == 1)
        {
            RvManager.instance.ClearRv(0);
            UpgradeManager.instance.SetIncomeMultiplier(1.5f, true);
        }
    }

    public void BuyCoinPack1()
    {
        EconomyManager.instance.IncreaseEconomy(3250);
    }

    public void BuyCoinPack2()
    {
        EconomyManager.instance.IncreaseEconomy(8400);
    }

    public void BuyCoinPack3()
    {
        EconomyManager.instance.IncreaseEconomy(22500);
    }

    public void BuyIncomeBundlePack()
    {
        // disable related Rvs effect before giving any reward
        RvManager.instance.ClearRv(0);
        
        EconomyManager.instance.IncreaseEconomy(3000);
        UpgradeManager.instance.SetIncomeMultiplier(1.5f, true);
        PlayerPrefs.SetInt(MyConstants.INCOME_BUNDLE_PURCHASED, 1);
        incomeBundleBuyButton.Interactable = false;
        incomeBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        incomeBundleCostText.text = "PURCHASED";
    }

    public void BuySpeedAndPowerPack()
    {
        // disable related Rvs effect before giving any reward
        RvManager.instance.ClearRv(3);
        
        EconomyManager.instance.IncreaseEconomy(500);
        UpgradeManager.instance.SetSpeedMultiplier(1.1f, true);
        UpgradeManager.instance.SetCriticalPowerMultiplier(1.1f, true);
        PlayerPrefs.SetInt(MyConstants.SPEED_POWER_BUNDLE_PURCHASED, 1);
        speedAndPowerBundleBuyButton.Interactable = false;
        speedAndPowerBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        speedAndPowerBundleCostText.text = "PURCHASED";
    }
}
