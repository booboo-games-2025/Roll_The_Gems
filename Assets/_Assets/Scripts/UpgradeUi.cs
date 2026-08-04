using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUi : MonoBehaviour
{
    [Header("Setup")]
    public UpgradeType upgradeType;
    
    [Header("UI")]
    [SerializeField] private TMP_Text costText, valueText;

    [SerializeField] private GameObject upgradeButtons, maxButton;
    [SerializeField] private UiButton upgradeBtn, upgradeRvBtn;
    [SerializeField] private Image Icon;
    [SerializeField] private Image upgradeLevelFillBar;
    [SerializeField] private Sprite enableSprite, disableSprite;

    private string _upgradeTypeEventName;

    private void Awake()
    {
        _upgradeTypeEventName = GetUpgradeTypeEventName();
    }

    public void Start()
    {
        Icon.sprite = GlobalvariableContainer.Instance.ballIcons[UpgradeManager.tabIndex];

        upgradeBtn.clickEvent.AddListener(() =>
        {
            HandleUpgrade(pIsUsingRv: false);
        });

        upgradeRvBtn.clickEvent.AddListener(() =>
        {
            HandleUpgrade(pIsUsingRv: true);
        });
    }

    private void HandleUpgrade(bool pIsUsingRv)
    {
        if (pIsUsingRv)
        {
            HCSDKManager.INSTANCE.DisplayRV(_upgradeTypeEventName, () =>
            {
                GameAnalyticsController.Miscellaneous.NewDesignEvent(_upgradeTypeEventName);
                UpgradeManager.instance.Upgrade(upgradeType, pIsUsingRv);
            });
        }
        else
        {
            UpgradeManager.instance.Upgrade(upgradeType, pIsUsingRv);
        }
    }

    private string GetUpgradeTypeEventName()
    {
        return upgradeType switch
        {
            UpgradeType.Income => MyConstants.INCOME_UPGRADE_RV,
            UpgradeType.CriticalHitChance => MyConstants.CRITICAL_CHANCE_UPGRADE_RV,
            UpgradeType.CriticalHitPower => MyConstants.CRITICAL_POWER_UPGRADE_RV,
            UpgradeType.Speed => MyConstants.SPEED_UPGRADE_RV,
            UpgradeType.BallCreationSpeed => MyConstants.CREATION_TIME_UPGRADE_RV,
            UpgradeType.Durability => MyConstants.DURABILITY_UPGRADE_RV,
            _ => "",
        };
    }

    public void UpdateUi(double cost, double value, int level)
    {
        Icon.sprite = GlobalvariableContainer.Instance.ballIcons[UpgradeManager.tabIndex];
        upgradeLevelFillBar.fillAmount = (level % 25)/25f;
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
                value *= UpgradeManager.CriticalPowerMultiplier;
            }
            // =======================================
            
            valueText.text = NumberFormatter.FormatNumberSmall(value) + "%";
        }
        else if (upgradeType == UpgradeType.CriticalHitChance)
        {
            // =======================================
            // if related Rv or IAP Active
            if (UpgradeManager.CriticalChanceMultiplierActive)
            {
                value *= UpgradeManager.CriticalChanceMultiplier;
            }
            // =======================================
            
            valueText.text = NumberFormatter.FormatNumberSmall(value) + "%";
            upgradeLevelFillBar.fillAmount = level/25f;
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
            
            valueText.text = NumberFormatter.FormatNumberSmall(value) + "s";
            upgradeLevelFillBar.fillAmount = level/25f;
            
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
            
            valueText.text = NumberFormatter.FormatNumberSmall(value);
            upgradeLevelFillBar.fillAmount = level/25f;
            
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
            
            valueText.text = "" + (int)value;
            
            // =======================================
            // if Infinite Durability Rv Active
            if (DurabilityInfiniteRv.IsActive)
            {
                valueText.text = "∞";
            }
            // =======================================
        }
        
    }
    
    public void SwitchButton(bool pIsAffordable, bool isMax = false)
    {
        if (isMax)
        {
            upgradeButtons.SetActive(false);
            maxButton.SetActive(true);
            return;
        }
        maxButton.SetActive(false);
        upgradeButtons.SetActive(true);
        
        upgradeBtn.gameObject.SetActive(pIsAffordable);
        upgradeRvBtn.gameObject.SetActive(!pIsAffordable);

        upgradeBtn.Interactable = pIsAffordable;
        upgradeBtn.image.sprite = pIsAffordable ? enableSprite : disableSprite;
    }
}
