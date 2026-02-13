using System;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FtueManager : MonoBehaviour
{
    public static FtueManager Instance;
    
    [SerializeField] private string unlockText, incomeUpgradeText, achievementButtonText, achievementDetailText;
    
    [SerializeField] CanvasGroup tutorialCanvasGroup; 
    [SerializeField] RectTransform highlight;
    [SerializeField] private Image masked;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private RectTransform speechBox;
    [SerializeField] private RectTransform hand;
    [SerializeField] private Image[] uiButtonToDisable;

    [SerializeField] private UiButton tapToContinueButton;
    [SerializeField] private UiButton unlockButton ,incomeButton, achievementButton;
    [SerializeField] private Transform unlockButtonTransform, incomeButtonTransform, achievementPanelPointerPos, achievememtPanel;
    public bool isFtueRunning;
    [SerializeField] private RectTransform pointer;


    private void Awake()
    {
        Instance = this;
        if(PlayerPrefs.GetInt(MyConstants.StartFtueCompleted,0) == 0)
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                StartTutorial();
            });
        }
    }

    void StartTutorial()
    {
        EnableDisableUiButtons(false);
        tutorialCanvasGroup.gameObject.SetActive(true);
        tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.interactable = true;
            tutorialCanvasGroup.blocksRaycasts = true;
        });
        ShowHighlight(unlockButtonTransform, new Vector2(400,180));
        ShowTutorialText(unlockButtonTransform,unlockText, new Vector2(0,50f));
        unlockButton.clickEvent.AddListener(()=>
        {
            TapOnIncomeButton();
        });
    }

    void TapOnIncomeButton()
    {
        unlockButton.clickEvent.RemoveListener(()=>
        {
            TapOnIncomeButton();
        });
        EconomyManager.instance.IncreaseEconomy(10);
        ShowHighlight(incomeButtonTransform, new Vector2(250,100));
        ShowTutorialText(incomeButtonTransform,incomeUpgradeText, new Vector2(0,50f));
        incomeButton.clickEvent.AddListener(()=>
        {
            EndIncomeTutorial();
        });
    }


    void EndIncomeTutorial()
    {
        EnableDisableUiButtons(true);
        PlayerPrefs.SetInt(MyConstants.StartFtueCompleted, 1);
        incomeButton.clickEvent.RemoveListener((() =>
        {
            EndIncomeTutorial();
        }));
        tutorialCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.gameObject.SetActive(false);
            tutorialCanvasGroup.interactable = false;
            tutorialCanvasGroup.blocksRaycasts = false;
        });
    }

    void StartAchivementTutorial()
    {
        if(PlayerPrefs.GetInt(MyConstants.AchievementFtueCompleted,0) == 0)
        {
            TutorialPanelPointerSwitch(false);
            tutorialCanvasGroup.gameObject.SetActive(true);
            tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                tutorialCanvasGroup.interactable = true;
                tutorialCanvasGroup.blocksRaycasts = true;
            });
            ShowHighlight(achievementButton.transform, new Vector2(150,150));
            ShowTutorialText(achievementButton.transform,achievementButtonText, new Vector2(30,-30f));
            achievementButton.clickEvent.AddListener(()=>
            {
                ShowAchievementPanelDetail();
            });
        }
    }

    void ShowAchievementPanelDetail()
    {
        achievementButton.clickEvent.RemoveListener(()=>
        {
            EndIncomeTutorial();
        });
        print("AchievementPanelShown");
        TutorialPanelPointerSwitch(true);
        ShowHighlight(achievememtPanel, new Vector2(900,1100));
        ShowTutorialText(achievementPanelPointerPos,achievementDetailText, new Vector2(0,50f));
        tapToContinueButton.gameObject.SetActive(true);
        tapToContinueButton.clickEvent.AddListener((() =>
        {
            EndAchievementTutorial();
        }));
    }

    void EndAchievementTutorial()
    {
        tapToContinueButton.clickEvent.RemoveListener((() =>
        {
            EndAchievementTutorial();
        }));
        EnableDisableUiButtons(true);
        tapToContinueButton.gameObject.SetActive(false);
        PlayerPrefs.SetInt(MyConstants.AchievementFtueCompleted, 1);
        tutorialCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.gameObject.SetActive(false);
            tutorialCanvasGroup.interactable = false;
            tutorialCanvasGroup.blocksRaycasts = false;
        });
    }

    public void ShowHighlight(Transform _target, Vector2 _size)
    {
        masked.gameObject.SetActive(true);
        highlight.gameObject.SetActive(true);
        highlight.sizeDelta = _size;
        highlight.position = _target.position;
    }

    public void ShowTutorialText(Transform target,string _text, Vector3 offset)
    {
        pointer.position = target.position + offset;
        tutorialText.text = _text;
    }

    void TutorialPanelPointerSwitch(bool down)
    {
        if (down)
        {
            pointer.rotation = Quaternion.identity;
            speechBox.localRotation = Quaternion.identity;
            speechBox.pivot = new Vector2(1, 0);
            speechBox.anchorMax = new Vector2(0, 1);
            speechBox.anchorMin = new Vector2(0, 1);
            speechBox.anchoredPosition = new Vector2(10,-10);
        }
        else
        {
            pointer.rotation = Quaternion.Euler(0, 0, 180);
            speechBox.localRotation = Quaternion.Euler(0, 0, 180);
            speechBox.pivot = new Vector2(0.5f, 0.5f);
            speechBox.anchorMax = new Vector2(0.5f, 0.5f);
            speechBox.anchorMin = new Vector2(0.5f, 0.5f);
            speechBox.anchoredPosition = new Vector2(-240, 140);
            //speechBox.anchoredPosition = new Vector2(-270, -10);
        }
    }

    void EnableDisableUiButtons(bool state)
    {
        foreach (var btn in uiButtonToDisable)
        {
            btn.raycastTarget = state;
        }
    }
}
