using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SkinButtonState
{
    Lock ,Unlock ,Selected
}

public class SkinButton : MonoBehaviour
{
    public int skinIndex;
    public Image BackgroundImage;
    public GameObject InUsePanelObject;
    public GameObject rvBuyButton;
    public CanvasGroup canvasGroup;
    public TMP_Text timerText;
    public int SkinCost;
    public TextMeshProUGUI SkinCostText;

    public SkinButtonState state;
    
    private void Start()
    {
        //SkinCostText.text = "<sprite=0> " + SkinCost;
    }

    public void UpdateTimer(float elaspedTime)
    {
        int minutes =  Mathf.FloorToInt(elaspedTime / 60);
        int seconds =  Mathf.FloorToInt(elaspedTime % 60);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void ChangeUiAccordingToState()
    {
        InUsePanelObject.SetActive(false);
        rvBuyButton.SetActive(false);
        //BackgroundImage.sprite = HoleSkinsManager.instance.unselectedBg;
        switch (state)
        {
            case SkinButtonState.Lock:
                rvBuyButton.SetActive(true);
                canvasGroup.alpha = 0.3f;
                canvasGroup.blocksRaycasts = false;
                break;
            case SkinButtonState.Unlock:
                rvBuyButton.SetActive(true);
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
                break;
            case SkinButtonState.Selected:
                InUsePanelObject.SetActive(true);
                //BackgroundImage.sprite = HoleSkinsManager.instance.selectedBg;
                break;
        }
    }

    public void BuyRvButtonClicked()
    {
        Unlock();
        //HCSDKManager.INSTANCE.DisplayRV(HCSDKManager.RV_LOAD_NAME,Unlock);
    }

    void Unlock()
    {
        HoleSkinsManager.instance.SetGemsSkin(skinIndex);
        string newEvent = "rv:unlock_skin" + "_" + skinIndex;
    }

}
