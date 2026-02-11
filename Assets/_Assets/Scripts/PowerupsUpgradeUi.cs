using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerupsUpgradeUi : MonoBehaviour
{
    
    [Header("Setup")]
    public int ballId;
    public UpgradeType upgradeType;
    
    [Header("UI")]
    [SerializeField] private TMP_Text costText, valueText;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] UiButton upgradeBtn;
    [SerializeField] private Image Icon;
    [SerializeField] private Image upgradeLevelFillBar;

    public void Start()
    {
        Icon.sprite = GlobalvariableContainer.Instance.ballIcons[ballId];
        upgradeBtn.clickEvent.AddListener(() => { PowerupsManager.instance.Upgrade(ballId, upgradeType); });
    }

    private bool isActive;
    public void Active(bool state)
    {
        isActive = state;
        canvasGroup.gameObject.SetActive(state);
    }

    public void UpdateUi(double cost, double value, int level)
    {
        costText.text = "<Sprite=0> " + NumberFormatter.FormatNumberSmall(cost);
        if (upgradeType == UpgradeType.Income)
        {
            valueText.text = "<Sprite=0> " + NumberFormatter.FormatNumberSmall(value);
        }
        else if (upgradeType == UpgradeType.CriticalHitChance || upgradeType == UpgradeType.CriticalHitPower)
        {
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture) + "%";
        }
        else if (upgradeType == UpgradeType.BallCreationSpeed)
        {
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture) + "s";
        }
        else
        {
            valueText.text = ((float)value).ToString(CultureInfo.InvariantCulture);
        }
        upgradeLevelFillBar.fillAmount = (level % 25)/25f;
    }

    public void SwitchButton(bool hasMoneyAvailable)
    {
        upgradeBtn.Interactable = hasMoneyAvailable;
        if (isActive)
        {
            canvasGroup.alpha = hasMoneyAvailable ? 1 : 0.4f;
        }
        //upgradeBtn.image.sprite = hasMoneyAvailable ? enableSprite : disableSprite;
    }
}
