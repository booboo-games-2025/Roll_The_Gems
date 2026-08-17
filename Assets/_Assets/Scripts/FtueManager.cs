using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FtueManager : MonoBehaviour
{
    public static FtueManager Instance;
    
    [SerializeField] private string unlockText, upgradeInfoText, upgradeGoalText, incomeUpgradeText, achievementButtonText, achievementDetailText, goalText;
    
    [SerializeField] CanvasGroup tutorialCanvasGroup; 
    [SerializeField] RectTransform highlight;
    [SerializeField] private Image masked;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private RectTransform speechBox;
    [SerializeField] private RectTransform hand;
    [SerializeField] private Image[] uiButtonToDisable;

    [SerializeField] private UiButton tapToContinueButton;
    [SerializeField] private UiButton unlockButton ,incomeButton, achievementButton;
    [SerializeField] private Transform unlockButtonTransform, upgradePanelTransform, incomeButtonTransform, achievementPanelPointerPos, achievememtPanel, goalHighlightTransform;
    public bool isFtueRunning;

    [SerializeField] private RectTransform pointer;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Scrollbar scrollbar;

    [Header("New Tutorial")]
    [SerializeField] private string _btcWalletText;
    [SerializeField] private Transform _btcWalletHighlightTransform;
    [SerializeField] private RectTransform _tutorialTextTransform;

    private Vector2 _tapToContinueTextOriginalPosition;

    private RectTransform _tapToContinueButtonRect;


    private void Awake()
    {
        Instance = this;

        _tapToContinueButtonRect = tapToContinueButton.GetComponent<RectTransform>();

        _tapToContinueTextOriginalPosition = _tapToContinueButtonRect.anchoredPosition;

        AchievementUi.OnAchievementComplete += StartAchivementTutorial;
        if(PlayerPrefs.GetInt(MyConstants.StartFtueCompleted,0) == 0)
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                ShowGoalTutorial();
            });
        }
    }

    private void ShowGoalTutorial()
    {
        EnableDisableUiButtons(false);
        tutorialCanvasGroup.gameObject.SetActive(true);
        tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.interactable = true;
            tutorialCanvasGroup.blocksRaycasts = true;
        });

        ShowHighlight(goalHighlightTransform, new Vector2(1100, 1150));
        ShowTutorialText(goalHighlightTransform, goalText, new Vector2(0, 0f));

        _tapToContinueButtonRect.anchoredPosition = _tapToContinueTextOriginalPosition;

        _tapToContinueButtonRect.anchoredPosition = new Vector2(0f, 1020f);

        tapToContinueButton.gameObject.SetActive(true);

        tapToContinueButton.clickEvent.AddListener(StartTutorial);
    }

    private void StartTutorial()
    {
        EnableDisableUiButtons(false);
        tapToContinueButton.gameObject.SetActive(false);
        tapToContinueButton.clickEvent.RemoveListener(StartTutorial);

        //tutorialCanvasGroup.gameObject.SetActive(true);
        //tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        //{
        //    tutorialCanvasGroup.interactable = true;
        //    tutorialCanvasGroup.blocksRaycasts = true;
        //});

        ShowHighlight(unlockButtonTransform, new Vector2(400,180));
        ShowTutorialText(unlockButtonTransform,unlockText, new Vector2(0,50f));
        unlockButton.clickEvent.AddListener(ShowUpgradeScrolling);
    }
    
    void ShowUpgradeScrolling()
    {
        if (PlayerPrefs.GetInt(MyConstants.StartFtueCompleted) == 1)
        {
            return;
        }
        scrollRect.vertical = false;
        unlockButton.clickEvent.RemoveListener(ShowUpgradeScrolling);
        DOTween.To(()=>scrollbar.value,x => scrollbar.value = x, 0, 2f).SetDelay(0.5f).OnComplete(() =>
        {
            _tapToContinueButtonRect.anchoredPosition = _tapToContinueTextOriginalPosition;

            _tapToContinueButtonRect.anchoredPosition += Vector2.up * 550;
            tapToContinueButton.gameObject.SetActive(true);
            tapToContinueButton.clickEvent.AddListener(UpgradesTutorial);
        });
        ShowHighlight(upgradePanelTransform, new Vector2(1100,680));
        ShowTutorialText(upgradePanelTransform,upgradeInfoText, new Vector2(0,250f));
    }

    private void UpgradesTutorial()
    {
        if (PlayerPrefs.GetInt(MyConstants.StartFtueCompleted) == 1)
        {
            return;
        }

        incomeButton.clickEvent.RemoveListener(UpgradesTutorial);
        ShowHighlight(upgradePanelTransform, new Vector2(1100, 680));
        ShowTutorialText(upgradePanelTransform, upgradeGoalText, new Vector2(0, 250f));

        tapToContinueButton.gameObject.SetActive(true);
        tapToContinueButton.clickEvent.AddListener(TapOnIncomeButton);
    }

    void TapOnIncomeButton()
    {
        if (PlayerPrefs.GetInt(MyConstants.StartFtueCompleted) == 1)
        {
            return;
        }
        scrollbar.value = 1;
        tapToContinueButton.gameObject.SetActive(false);
        tapToContinueButton.clickEvent.RemoveListener(TapOnIncomeButton);
        EconomyManager.instance.IncreaseEconomy(10);
        ShowHighlight(incomeButtonTransform, new Vector2(250,100));
        ShowTutorialText(incomeButtonTransform,incomeUpgradeText, new Vector2(0,50f));
        incomeButton.clickEvent.AddListener(ShowBtcWalletTutorial);
    }

    // New Tutorial
    private void ShowBtcWalletTutorial()
    {
        incomeButton.clickEvent.RemoveListener(ShowBtcWalletTutorial);
        EnableDisableUiButtons(false);
        tutorialCanvasGroup.gameObject.SetActive(true);
        tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.interactable = true;
            tutorialCanvasGroup.blocksRaycasts = true;
        });

        TutorialPanelPointerSwitch(false);
        ShowHighlight(_btcWalletHighlightTransform);
        ShowTutorialText(_btcWalletHighlightTransform, _btcWalletText, new Vector2(0, -50f));

        _tapToContinueButtonRect.anchoredPosition = _tapToContinueTextOriginalPosition;

        _tapToContinueButtonRect.anchoredPosition = new Vector2(0f, 1020f);
        tapToContinueButton.gameObject.SetActive(true);
        tapToContinueButton.clickEvent.AddListener(EndIncomeTutorial);
    }

    void EndIncomeTutorial()
    {
        scrollRect.vertical = true;
        EnableDisableUiButtons(true);
        PlayerPrefs.SetInt(MyConstants.StartFtueCompleted, 1);
        UpgradeManager.instance.Save();
        EconomyManager.instance.SaveEconomy();
        incomeButton.clickEvent.RemoveListener(EndIncomeTutorial);
        tutorialCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.gameObject.SetActive(false);
            tutorialCanvasGroup.interactable = false;
            tutorialCanvasGroup.blocksRaycasts = false;
        });

        // Start another FTUE from here
        InAppsManager.Instance.baseInAppsManager.ApplyPlayerBonuses();
        GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.GA_STARTING_FTUE);
    }

    void StartAchivementTutorial()
    {
        if(PlayerPrefs.GetInt(MyConstants.AchievementFtueCompleted,0) == 0)
        {
            EnableDisableUiButtons(false);
            TutorialPanelPointerSwitch(false);
            tutorialCanvasGroup.gameObject.SetActive(true);
            tutorialCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                tutorialCanvasGroup.interactable = true;
                tutorialCanvasGroup.blocksRaycasts = true;
            });
            ShowHighlight(achievementButton.transform, new Vector2(150,150));
            ShowTutorialText(achievementButton.transform,achievementButtonText, new Vector2(30,-30f));
            achievementButton.Interactable = true;
            achievementButton.clickEvent.AddListener(ShowAchievementPanelDetail);
        }
    }

    void ShowAchievementPanelDetail()
    {
        achievementButton.clickEvent.RemoveListener(ShowAchievementPanelDetail);
        print("AchievementPanelShown");

        TutorialPanelPointerSwitch(true);
        ShowHighlight(achievememtPanel, new Vector2(900,1100));
        ShowTutorialText(achievementPanelPointerPos,achievementDetailText, new Vector2(0,50f));
        tapToContinueButton.gameObject.SetActive(true);
        tapToContinueButton.clickEvent.AddListener(EndAchievementTutorial);
    }

    void EndAchievementTutorial()
    {
        tapToContinueButton.clickEvent.RemoveListener(EndAchievementTutorial);
        EnableDisableUiButtons(true);
        tapToContinueButton.gameObject.SetActive(false);
        PlayerPrefs.SetInt(MyConstants.AchievementFtueCompleted, 1);
        tutorialCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            tutorialCanvasGroup.gameObject.SetActive(false);
            tutorialCanvasGroup.interactable = false;
            tutorialCanvasGroup.blocksRaycasts = false;
        });
        GameAnalyticsController.Miscellaneous.NewDesignEvent(MyConstants.GA_ACHIEVEMENT_FTUE);
    }

    public void ShowHighlight(Transform _target, Vector2 _size = default(Vector2))
    {
        masked.gameObject.SetActive(true);
        highlight.gameObject.SetActive(true);

        if (_size != default(Vector2))
        {
            highlight.sizeDelta = _size;
        }
        highlight.position = _target.position;
    }

    public void ShowTutorialText(Transform target,string _text, Vector3 offset)
    {
        //pointer.position = target.position + offset;
        pointer.position = target.position;
        tutorialText.text = _text;
    }

    void TutorialPanelPointerSwitch(bool down)
    {
        Debug.Log("Tutorial panel pointer swuitch");
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
            pointer.rotation = Quaternion.identity;
            speechBox.localRotation = Quaternion.identity;
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
