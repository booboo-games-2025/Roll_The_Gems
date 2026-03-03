using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

internal class GenericPopUp : MonoBehaviour
{
    public static GenericPopUp Instance;
    [SerializeField] Popup popup;
    private Image backgroundImage;
    
    private void Awake()
    {
        Instance = this;
        backgroundImage = GetComponent<Image>();
        popup.transform.DOScale(Vector3.zero, 0f).SetEase(Ease.OutCubic);
    }
    
    internal void ShowPopUp(PopUpType type, string description, string btnTitle)
    {
        popup.gameObject.SetActive(true);
        popup.Type = type;
        popup.Description = description;
        popup.BtnTitle = btnTitle;
        AnimateIn();
    }

    private void AnimateIn()
    {
        backgroundImage.DOFade(0.9f, 0.6f).SetEase(Ease.OutCubic);
        popup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCubic);
    }

    public void ClosePopUp()
    {

        //UImanager.instance.PlayMusicAndHapticUI();
        backgroundImage.DOFade(0f, 0.1f).SetEase(Ease.InCubic);
        popup.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InCubic).OnComplete(() => popup.gameObject.SetActive(false));
    }

    
    public void ShowBtn(PopUpType type, string desc, string btn)
    {
        ShowPopUp(type, desc, btn);
    }
}

public enum PopUpType
{
    Alert,
    Blank
}