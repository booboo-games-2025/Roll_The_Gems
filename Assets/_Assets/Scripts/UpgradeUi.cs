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
    [SerializeField] UiButton upgradeBtn;
    [SerializeField] private Image Icon;
    [SerializeField] private Image upgradeLevelFillBar;
    
    public void Start()
    {
        Icon.sprite = GlobalvariableContainer.Instance.ballIcons[UpgradeManager.tabIndex];
        upgradeBtn.clickEvent.AddListener(() => { UpgradeManager.instance.Upgrade(UpgradeManager.tabIndex, upgradeType); });
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
        else if (upgradeType == UpgradeType.CriticalHitChance || upgradeType == UpgradeType.CriticalHitPower)
        {
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
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture);
            if (DurabilityInfiniteRv.IsActive)
            {
                valueText.text = "∞";
            }
        }
        upgradeLevelFillBar.fillAmount = (level % 25)/25f;
    }

    [SerializeField] private Sprite enableSprite, disableSprite;
    public void SwitchButton(bool hasMoneyAvailable)
    {
        upgradeBtn.Interactable = hasMoneyAvailable;
        upgradeBtn.image.sprite = hasMoneyAvailable ? enableSprite : disableSprite;
    }
}
