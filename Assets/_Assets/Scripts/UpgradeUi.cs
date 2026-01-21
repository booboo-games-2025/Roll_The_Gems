using System;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUi : MonoBehaviour
{
    public UiButton upgradeBtn;
    [SerializeField] private TMP_Text costText, incomeText;
    [SerializeField] private TMP_Text currentBallCount;
    [SerializeField] private Image fillImage, upgradeLevelFillBar;
    public GameObject content;
    [SerializeField] Sprite enableSprite, disableSprite;
    
    public void SwitchButton(bool hasMoneyAvailable)
    {
        upgradeBtn.Interactable = hasMoneyAvailable;
        upgradeBtn.image.sprite = hasMoneyAvailable ? enableSprite : disableSprite;
    }

    public void HandleBallFill(float duration)
    {
        fillImage.fillAmount = 0f;
        fillImage.DOFillAmount(1f, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            fillImage.fillAmount = 0f;
        });
    }

    public void HandleBallCount(int count)
    {
        currentBallCount.text = count.ToString();
    }

    public void UpdateCostAndIncome(double cost, double income, int level)
    {
        if (cost <= 0)
        {
            costText.text = "FREE";
        }
        else
        {
            costText.text = NumberFormatter.FormatNumberSmall(cost);
        }
        incomeText.text = NumberFormatter.FormatNumberSmall(income);
        upgradeLevelFillBar.fillAmount = (level % 25)/25f;
    }
}
