using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PowerupsUpgradeUi : MonoBehaviour
{
    
    [Header("Setup")]
    public int ballId;
    public PowerType powerType;
    
    [Header("UI")]
    [SerializeField] private TMP_Text costText;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] UiButton upgradeBtn;

    public void Start()
    {
        if (powerType == PowerType.IncomeMultiple)
        {
            upgradeBtn.clickEvent.AddListener( ()=>
            {
                PowerupsManager.instance.UpgradeAllIncome();
            });
        }
        else if (powerType == PowerType.RingHealthReduce)
        {
            upgradeBtn.clickEvent.AddListener( ()=>
            {
                PowerupsManager.instance.UpgradeRingHealth();
            });
        }
        else
        {
            upgradeBtn.clickEvent.AddListener( ()=>
            {
                PowerupsManager.instance.UpgradePower(ballId, powerType);
            });
        }
    }
    
    public void Active(bool state)
    {
        canvasGroup.alpha = state ? 1 : 0.5f;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public void UpdateUi(double cost)
    {
        costText.text = NumberFormatter.FormatNumberSmall(cost);
    }

    public void SwitchButton(bool hasMoneyAvailable)
    {
        upgradeBtn.Interactable = hasMoneyAvailable;
        Active(hasMoneyAvailable);
        //upgradeBtn.image.sprite = hasMoneyAvailable ? enableSprite : disableSprite;
    }
}
