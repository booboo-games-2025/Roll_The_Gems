using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject ShopPanel;
    
    [Header("IncomeBundleUi")]
    [SerializeField] UiButton incomeBundleBuyButton;
    [SerializeField] private TMP_Text incomeBundleCostText;
    
    [Header("Speed&PowerBundleUi")]
    [SerializeField] UiButton speedAndPowerBundleBuyButton;
    [SerializeField] private TMP_Text speedAndPowerBundleCostText;
    
    [Header("Speed&PowerBundleUi")]
    [SerializeField] UiButton megaUpgradeBundleBuyButton;
    [SerializeField] private TMP_Text megaUpgradeBundleCostText;
    
    private void Awake()
    {
        instance = this;
    }

    public void OpenShopPanle()
    {
        ShopPanel.SetActive(true);
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt(MyConstants.INCOME_BUNDLE_PURCHASED, 0) == 1)
        {
            BuyIncomeBundlePack(true);
        }
        
        if (PlayerPrefs.GetInt(MyConstants.SPEED_POWER_BUNDLE_PURCHASED, 0) == 1)
        {
            BuySpeedAndPowerPack(true);
        }
        
        if (PlayerPrefs.GetInt(MyConstants.MEGA_UPGRADE_BUNDEL_PURCHASED, 0) == 1)
        {
            BuyMegaUpgradePack(true);
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

    public void BuyIncomeBundlePack(bool isFromRestore)
    {
        // disable related Rvs effect before giving any reward
        if (isFromRestore == false)
        {
            EconomyManager.instance.IncreaseEconomy(3000);
            PlayerPrefs.SetInt(MyConstants.INCOME_BUNDLE_PURCHASED, 1);
        }
        RvManager.instance.ClearRv(4);
        UpgradeManager.instance.SetIncomeMultiplier(1.5f, true);
        
        incomeBundleBuyButton.Interactable = false;
        incomeBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        incomeBundleCostText.text = "PURCHASED";
    }

    public void BuySpeedAndPowerPack(bool isFromRestore)
    {
        // disable related Rvs effect before giving any reward
        if (isFromRestore == false)
        {
            EconomyManager.instance.IncreaseEconomy(500);
            PlayerPrefs.SetInt(MyConstants.SPEED_POWER_BUNDLE_PURCHASED, 1);
        }
        RvManager.instance.ClearRv(3);
        UpgradeManager.instance.SetSpeedMultiplier(1.1f, true);
        UpgradeManager.instance.SetCriticalPowerMultiplier(1.1f, true);
        
        speedAndPowerBundleBuyButton.Interactable = false;
        speedAndPowerBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        speedAndPowerBundleCostText.text = "PURCHASED";
    }
    
    public void BuyMegaUpgradePack(bool isFromRestore)
    {
        if (isFromRestore == false)
        {
            PlayerPrefs.SetInt(MyConstants.MEGA_UPGRADE_BUNDEL_PURCHASED, 1);
        }
        // disable related Rvs effect before giving any reward
        RvManager.instance.ClearRv(0);
        UpgradeManager.instance.SetCriticalChanceMultiplier(2f, true);
        UpgradeManager.instance.SetCriticalPowerMultiplier(2f, true);
        UpgradeManager.instance.SetDurabilityMultiplier(2, true);
        
        megaUpgradeBundleBuyButton.Interactable = false;
        megaUpgradeBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        megaUpgradeBundleCostText.text = "PURCHASED";
    }
}
