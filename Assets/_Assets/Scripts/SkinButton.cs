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
    public UiButton rvBuyButton;
    public TMP_Text timerText;

    public SkinButtonState state;

    public static Action<int> OnSkinChanged; 

    public void SetSkin()
    {
        OnSkinChanged?.Invoke(skinIndex);
        StartCoroutine(ResetGemsSkin());
    }
    
    IEnumerator ResetGemsSkin()
    {
        float elaspedTime = 300;
        while (elaspedTime > 0f)
        {
            UpdateTimer(elaspedTime);
            yield return new WaitForSeconds(1);
            elaspedTime--;
        }
        OnSkinChanged?.Invoke(skinIndex);
        state = SkinButtonState.Lock;
        ChangeUiAccordingToState();
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
        rvBuyButton.gameObject.SetActive(false);
        //BackgroundImage.sprite = HoleSkinsManager.instance.unselectedBg;
        switch (state)
        {
            case SkinButtonState.Lock:
                rvBuyButton.gameObject.SetActive(true);
                break;
            case SkinButtonState.Selected:
                InUsePanelObject.SetActive(true);
                //BackgroundImage.sprite = HoleSkinsManager.instance.selectedBg;
                break;
        }
    }
    

    public void BuyRvButtonClicked()
    {
        SetSkin();
        //HCSDKManager.INSTANCE.DisplayRV(HCSDKManager.RV_LOAD_NAME,Unlock);
    }

    void Unlock()
    {
       // HoleSkinsManager.instance.SetGemsSkin(skinIndex);
       // string newEvent = "rv:unlock_skin" + "_" + skinIndex;
    }
}
