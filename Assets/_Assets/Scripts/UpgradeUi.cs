using System;
using System.Globalization;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUi : MonoBehaviour
{
    [Header("Setup")]
    public UpgradeType upgradeType;
    
    [Header("UI")]
    [SerializeField] private TMP_Text costText, valueText;

    [SerializeField] private UiButton upgradeBtn, upgradeRvBtn;
    [SerializeField] private Image Icon;
    [SerializeField] private Image upgradeLevelFillBar;
    
    public void Start()
    {
        Icon.sprite = GlobalvariableContainer.Instance.ballIcons[UpgradeManager.tabIndex];
        upgradeBtn.clickEvent.AddListener(() => { UpgradeManager.instance.Upgrade(UpgradeManager.tabIndex, upgradeType); });
        upgradeRvBtn.clickEvent.AddListener(() => { HCSDKManager.INSTANCE.DisplayRV(HCSDKManager.RV_LOAD_NAME,RvButtonClicked); });
    }

    public void RvButtonClicked()
    {
        UpgradeManager.instance.Upgrade(UpgradeManager.tabIndex, upgradeType);
        if (upgradeType == UpgradeType.CriticalHitPower)
        {
            GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.CRITICAL_POWER_UPGRADE_RV);
        }
        else if (upgradeType == UpgradeType.Speed)
        {
            GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.SPEED_UPGRADE_RV);
        }
        else if (upgradeType == UpgradeType.BallCreationSpeed)
        {
            GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.CREATION_TIME_UPGRADE_RV);
        }
        else if (upgradeType == UpgradeType.Durability)
        {
            GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.DURABILITY_UPGRADE_RV);
        }
    }

    public void UpdateUi(double cost, double value, int level)
    {
        Icon.sprite = GlobalvariableContainer.Instance.ballIcons[UpgradeManager.tabIndex];
        if (cost == 0)
        {
            costText.text = "<Sprite=0> Free";
        }
        else
        {
            costText.text = "<Sprite=0> " + NumberFormatter.FormatNumberSmall(cost);
        }
        if (upgradeType == UpgradeType.Income)
        {
            // =======================================
            // if related Rv or IAP Active
            if (UpgradeManager.IncomeMultiplierActive)
            {
                value *= UpgradeManager.IncomeMultiplier;
            }
            // =======================================
            
            valueText.text = "<Sprite=0> " + NumberFormatter.FormatNumberSmall(value);
        }
        else if (upgradeType == UpgradeType.CriticalHitPower)
        {
            // =======================================
            // if related Rv or IAP Active
            if (UpgradeManager.CriticalPowerMultiplierActive)
            {
                value /= UpgradeManager.CriticalPowerMultiplier;
            }
            // =======================================
            
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture) + "%";
        }
        else if (upgradeType == UpgradeType.CriticalHitChance)
        {
            // =======================================
            // if related Rv or IAP Active
            if (UpgradeManager.CriticalChanceMultiplierActive)
            {
                value /= UpgradeManager.CriticalChanceMultiplier;
            }
            // =======================================
            
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture) + "%";
        }
        else if (upgradeType == UpgradeType.BallCreationSpeed)
        {
            // =======================================
            // if related Rv or IAP Active
            if (UpgradeManager.CreationSpeedMultiplierActive)
            {
                value /= UpgradeManager.CreationSpeedMuliplier;
            }
            // =======================================
            
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture) + "s";
        }
        else if(upgradeType == UpgradeType.Speed)
        {
            // =======================================
            // if related Rv or IAP Active
            if (UpgradeManager.SpeedMultiplierActive)
            {
                value *= UpgradeManager.SpeedMultiplier;
            }
            // =======================================
            
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            // =======================================
            // if related IAP Active
            if (UpgradeManager.DurabilityActive)
            {
                value *= UpgradeManager.DurabilityMultiplier;
            }
            // =======================================
            
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture);
            
            // =======================================
            // if Infinite Durability Rv Active
            if (DurabilityInfiniteRv.IsActive)
            {
                valueText.text = "∞";
            }
            // =======================================
        }
        upgradeLevelFillBar.fillAmount = (level % 25)/25f;
    }

    [SerializeField] private Sprite enableSprite, disableSprite;
    public void SwitchButton(bool hasMoneyAvailable, bool showRvButton = false)
    {
        upgradeBtn.gameObject.SetActive(true);
        upgradeRvBtn.gameObject.SetActive(false);
        upgradeBtn.Interactable = hasMoneyAvailable;
        upgradeBtn.image.sprite = hasMoneyAvailable ? enableSprite : disableSprite;
        if (showRvButton)
        {
            upgradeBtn.gameObject.SetActive(false);
            upgradeRvBtn.gameObject.SetActive(true);
        }
    }
}
