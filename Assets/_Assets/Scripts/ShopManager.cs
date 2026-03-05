using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject ShopPanel;
    [SerializeField] TMP_Text coinPackFirstText, coinPackSecondText, coinPackThirdText;
    
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
        SetPriceText();
    }

    void SetPriceText()
    {
        coinPackFirstText.text = InAppManagerUnityIAP_Boombit.COINS_PACK_1_PRICE;
        coinPackSecondText.text = InAppManagerUnityIAP_Boombit.COINS_PACK_2_PRICE;
        coinPackThirdText.text = InAppManagerUnityIAP_Boombit.COINS_PACK_3_PRICE;
        if (PlayerPrefs.GetInt(MyConstants.INCOME_BUNDLE_PURCHASED, 0) == 1)
        {
            incomeBundleBuyButton.Interactable = false;
            incomeBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
            incomeBundleCostText.text = "PURCHASED";
        }
        else
        {
            incomeBundleCostText.text = InAppManagerUnityIAP_Boombit.INCOME_PACK_PRICE;
        }
        
        if (PlayerPrefs.GetInt(MyConstants.SPEED_POWER_BUNDLE_PURCHASED, 0) == 1)
        {
            speedAndPowerBundleBuyButton.Interactable = false;
            speedAndPowerBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
            speedAndPowerBundleCostText.text = "PURCHASED";
        }
        else
        {
            speedAndPowerBundleCostText.text = InAppManagerUnityIAP_Boombit.SPEED_BUNDLE_PACK_PRICE;
        }
        
        if (PlayerPrefs.GetInt(MyConstants.MEGA_UPGRADE_BUNDEL_PURCHASED, 0) == 1)
        {
            megaUpgradeBundleBuyButton.Interactable = false;
            megaUpgradeBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
            megaUpgradeBundleCostText.text = "PURCHASED";
        }
        else
        {
            megaUpgradeBundleCostText.text = InAppManagerUnityIAP_Boombit.MEGA_UPGRADE_BUNDLE_PACK_PRICE;
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
        incomeBundleBuyButton.Interactable = false;
        incomeBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        incomeBundleCostText.text = "PURCHASED";
        print("Sachin Income Bundle Pack");
        //RvManager.instance.ClearRv(0);
        RvManager.instance.RemoveTwoxIncomeRv();
        UpgradeManager.instance.SetIncomeMultiplier(1.25f, true);
        PlayerPrefs.SetInt(MyConstants.INCOME_BUNDLE_PURCHASED, 1);
        EconomyManager.instance.IncreaseEconomy(3000);
    }

    public void BuySpeedAndPowerPack()
    {
        speedAndPowerBundleBuyButton.Interactable = false;
        speedAndPowerBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        speedAndPowerBundleCostText.text = "PURCHASED";
        print("Sachin Speed and Power Bundle Pack");
        EconomyManager.instance.IncreaseEconomy(500);
        PlayerPrefs.SetInt(MyConstants.SPEED_POWER_BUNDLE_PURCHASED, 1);
        //RvManager.instance.ClearRv(4);
        RvManager.instance.RemoveTwoxBallSpeedRv();
        UpgradeManager.instance.SetSpeedMultiplier(1.1f, true);
        UpgradeManager.instance.SetCriticalPowerMultiplier(1.1f, true);
    }
    
    public void BuyMegaUpgradePack()
    {
        megaUpgradeBundleBuyButton.Interactable = false;
        megaUpgradeBundleBuyButton.GetComponent<Image>().sprite = GlobalvariableContainer.Instance.disableSprite;
        megaUpgradeBundleCostText.text = "PURCHASED";
        print("Sachin Mega Upgrade Pack");
        PlayerPrefs.SetInt(MyConstants.MEGA_UPGRADE_BUNDEL_PURCHASED, 1);
        UpgradeManager.instance.SetCriticalChanceMultiplier(2f, true);
        UpgradeManager.instance.SetCriticalPowerMultiplier(2f, true);
        UpgradeManager.instance.SetDurabilityMultiplier(2, true);
    }
}
