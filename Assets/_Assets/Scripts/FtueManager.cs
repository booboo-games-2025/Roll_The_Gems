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
    
    [SerializeField] private string incomeUpgradeText;
    
    [SerializeField] CanvasGroup tutorialCanvasGroup; 
    [SerializeField] RectTransform highlight;
    [SerializeField] private Image masked;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private RectTransform speechBox;
    [SerializeField] private RectTransform hand;
    [SerializeField] private Image[] uiButtonToDisable;

    [SerializeField] private UiButton incomeButton, achievementButton;
    [SerializeField] private Transform incomeButtonTransform;
    public bool isFtueRunning;
    [SerializeField] private RectTransform pointer;
    
    const string ftueCompleted = "ftue_completed";
    const string achievementCompleted = "achievement_completed";

    private void Awake()
    {
        Instance = this;
        if(PlayerPrefs.GetInt("ftue_completed",0) == 0)
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                StartIncomeTutorial();
            });
        }
    }

    void StartIncomeTutorial()
    {
        EnableDisableUiButtons(false);
        tutorialCanvasGroup.gameObject.SetActive(true);
        tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.interactable = true;
            tutorialCanvasGroup.blocksRaycasts = true;
        });
        ShowHighlight(incomeButtonTransform, new Vector2(250,160));
        ShowTutorialText(incomeButtonTransform,incomeUpgradeText, new Vector2(0,50f));
        incomeButton.clickEvent.AddListener(()=>
        {
            EndIncomeTutorial();
        });
    }


    void EndIncomeTutorial()
    {
        EnableDisableUiButtons(true);
        PlayerPrefs.SetInt("ftue_completed", 1);
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
        if(PlayerPrefs.GetInt(achievementCompleted,0) == 0)
        {
            tutorialCanvasGroup.gameObject.SetActive(true);
            tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                tutorialCanvasGroup.interactable = true;
                tutorialCanvasGroup.blocksRaycasts = true;
            });
            ShowHighlight(incomeButton.transform, new Vector2(250,160));
            ShowTutorialText(incomeButton.transform,incomeUpgradeText, new Vector2(0,50f));
            incomeButton.clickEvent.AddListener(()=>
            {
                EndIncomeTutorial();
            });
        }
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
            speechBox.anchoredPosition = new Vector2(10, -10);
        }
        else
        {
            pointer.rotation = Quaternion.Euler(0, 0, 90);
            speechBox.localRotation = Quaternion.Euler(0, 0, -90);
            speechBox.anchoredPosition = new Vector2(-270, -10);
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
