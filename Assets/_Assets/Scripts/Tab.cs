using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Tab : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite seletecSprite, deselectedSprite;
    public UiButton uiButton;
    public Image mainIcon;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text currentBallCount;
    public GameObject exclaimMark;

    private Color _mainIconColor;
    private void Awake()
    {
        _mainIconColor = Color.white;
        _mainIconColor.a = 0.25f;
    }

    public void SwitchSprite(bool flag)
    {
        img.sprite = flag ? seletecSprite : deselectedSprite;
        //uiButton.Interactable = !flag;
    }
    
    public void HandleBallFill(float duration)
    {
        mainIcon.color = _mainIconColor;
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
}
