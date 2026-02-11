using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public double totalEarnCoin;
    public int createBallsCount, destroyRingsCount, buyUpgradesCount, criticalIncomeCount, offlineIncomeCount;
    public float gameTime;

    [SerializeField] private AchievementUi[] earningAchievementsUi, createBallsUi, destroyRingsUi, buyUpgradeUi, criticalIncomeUi, playGameUi,
        offlineIncomeUi;
    
    public static Action<double, AchievementType> OnAchievementsUpdated;

    [Header("UI")] [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text bonusText;

    private void Awake()
    {
        OnAchievementsUpdated += UpdateProgress;
        AchievementUi.OnAchievementComplete += UpdateUi;
    }

    private void Start()
    {
        LoadValues();
        if (gameTime <= 7200)
        {
            StartCoroutine(GameTimer());
        }
    }

    void LoadValues()
    {
        totalEarnCoin = EconomyManager.GetDouble(MyConstants.EARNED_INCOME);
        createBallsCount = PlayerPrefs.GetInt(MyConstants.CREATE_BALL, 0);
        destroyRingsCount = PlayerPrefs.GetInt(MyConstants.DESTROY_RINGS,0);
        buyUpgradesCount = PlayerPrefs.GetInt(MyConstants.UPGRADE_TIMES, 0);
        criticalIncomeCount = PlayerPrefs.GetInt(MyConstants.CRITICAL_TIMES,0);
        gameTime = PlayerPrefs.GetFloat(MyConstants.GAMEPLAY_TIME, 0);
        offlineIncomeCount = PlayerPrefs.GetInt(MyConstants.OFFLINE_INCOME_COUNT, 0);
        UpdateIncomeUi();
        UpdateCreateBallUi();
        UpdateDestroyRingsUi();
        UpdateUpgradesUi();
        UpdateCriticalIncomeUi();
        UpdateGameTimeUi();
        UpdateOfflineIncomeUi();
    }

    IEnumerator GameTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            UpdateProgress(1, AchievementType.PlayGameTime);
        }
    }

    public void UpdateProgress(double value,AchievementType achievementType)
    {
        if (achievementType == AchievementType.EarnCoin)
        {
            totalEarnCoin += value;
            EconomyManager.SetDouble(MyConstants.EARNED_INCOME, totalEarnCoin);
            UpdateIncomeUi();
        }
        else if (achievementType == AchievementType.CreateBalls)
        {
            createBallsCount += (int)value;
            PlayerPrefs.SetInt(MyConstants.CREATE_BALL, createBallsCount);
            UpdateCreateBallUi();
        }
        else if (achievementType == AchievementType.DestroyRings)
        {
            destroyRingsCount +=  (int)value;
            PlayerPrefs.SetInt(MyConstants.DESTROY_RINGS, destroyRingsCount);
            UpdateDestroyRingsUi();
        }
        else if (achievementType == AchievementType.BuyUpgradesXTimes)
        {
            buyUpgradesCount += (int)value;
            PlayerPrefs.SetInt(MyConstants.UPGRADE_TIMES, buyUpgradesCount);
            UpdateUpgradesUi();
        }
        else if (achievementType == AchievementType.GetCriticalIncomeXTime)
        {
            criticalIncomeCount += (int)value;
            PlayerPrefs.SetInt(MyConstants.CRITICAL_TIMES, criticalIncomeCount);
            UpdateCriticalIncomeUi();
        }
        else if (achievementType == AchievementType.PlayGameTime)
        {
            gameTime += (float)value;
            PlayerPrefs.SetFloat(MyConstants.GAMEPLAY_TIME, gameTime);
            UpdateGameTimeUi();
        }
        else if (achievementType == AchievementType.GetOfflineIncomeXTime)
        {
            offlineIncomeCount += (int)value;
            PlayerPrefs.SetInt(MyConstants.OFFLINE_INCOME_COUNT, offlineIncomeCount);
            UpdateOfflineIncomeUi();
        }
    }

    public void UpdateIncomeUi()
    {
        for (int i = 0; i < earningAchievementsUi.Length; i++)
        {
            earningAchievementsUi[i].UpdateProgress(totalEarnCoin);
        }
    }
    
    public void UpdateCreateBallUi()
    {
        for (int i = 0; i < createBallsUi.Length; i++)
        {
            createBallsUi[i].UpdateProgress(createBallsCount);
        }
    }
    
    public void UpdateDestroyRingsUi()
    {
        for (int i = 0; i < destroyRingsUi.Length; i++)
        {
            destroyRingsUi[i].UpdateProgress(destroyRingsCount);
        }
    }
    
    public void UpdateUpgradesUi()
    {
        for (int i = 0; i < buyUpgradeUi.Length; i++)
        {
            buyUpgradeUi[i].UpdateProgress(buyUpgradesCount);
        }
    }
    
    public void UpdateCriticalIncomeUi()
    {
        for (int i = 0; i < criticalIncomeUi.Length; i++)
        {
            criticalIncomeUi[i].UpdateProgress(criticalIncomeCount);
        }
    }
    
    public void UpdateGameTimeUi()
    {
        for (int i = 0; i < playGameUi.Length; i++)
        {
            playGameUi[i].UpdateProgress(gameTime);
        }
    }
    
    public void UpdateOfflineIncomeUi()
    {
        for (int i = 0; i < offlineIncomeUi.Length; i++)
        {
            offlineIncomeUi[i].UpdateProgress(offlineIncomeCount);
        }
    }

    private int progress;
    public static Action OnAchievementCompleted;
    void UpdateUi()
    {
        progress++;
        OnAchievementCompleted?.Invoke();
        progressText.text = "Progress: " + progress + "/18";
        bonusText.text = "Bonus <sprite=0> " + (progress * 3) + "%";
    }
}

public enum AchievementType
{
    EarnCoin, CreateBalls, DestroyRings, BuyUpgradesXTimes, GetCriticalIncomeXTime, PlayGameTime, GetOfflineIncomeXTime
}
