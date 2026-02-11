using System;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class OfflineEarning : MonoBehaviour
{
    private readonly string playerPrefsKey = "TimeWhenPaused";

    [SerializeField] private TMP_Text offlineForText, rewardText;
    [SerializeField] private GameObject welcomeBackUi;

    private void Start()
    {
        if(!PlayerPrefs.HasKey(playerPrefsKey)) return;
        
        long binaryTime = Convert.ToInt64(PlayerPrefs.GetString(playerPrefsKey));
        DateTime lastTime = DateTime.FromBinary(binaryTime);

        TimeSpan timeSpan = DateTime.UtcNow - lastTime;
        HandleOnUserBackOnline(timeSpan);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(playerPrefsKey, DateTime.UtcNow.ToBinary().ToString());
    }
    
    private double moneyToAdd;
    public void HandleOnUserBackOnline(TimeSpan timeSpan)
    {
        double totalMinutes = timeSpan.TotalMinutes;

        if (totalMinutes < 1) return; // Less than a minute gets no reward
        
        welcomeBackUi.SetActive(true);
        int hours = (int)timeSpan.TotalHours;
        int minutes = timeSpan.Minutes;
        offlineForText.text = $"You’ve been away for {hours}h {minutes}m";
        
        // Give Money based on duration
        // Ref game does not add this reward towards Level progress
        totalMinutes = Mathf.Min((float)totalMinutes, 120f);
        moneyToAdd = totalMinutes * 50;
        rewardText.text = "<sprite=0> " + NumberFormatter.FormatNumberSmall(moneyToAdd);
    }
    
    public void ClaimOfflineEarning()
    {
        EconomyManager.instance.IncreaseEconomy(moneyToAdd);
        welcomeBackUi.SetActive(false);
        Achievements.OnAchievementsUpdated?.Invoke(1,AchievementType.GetOfflineIncomeXTime);
    }
    
    // Rv Func
    public void ClaimOfflineEarning2X()
    {
        EconomyManager.instance.IncreaseEconomy(moneyToAdd * 2);
        welcomeBackUi.SetActive(false);
        Achievements.OnAchievementsUpdated?.Invoke(1,AchievementType.GetOfflineIncomeXTime);
        //GameAnalyticsController.Miscellaneous.NewDesignEvent("rv:claim_offline_earning_2x");
    }

    /*private void OnApplicationPause(bool pPause)
    {
        if(pPause)
        {
            PlayerPrefs.SetString(playerPrefsKey, DateTime.Now.ToString());
        }
        else if(PlayerPrefs.HasKey(playerPrefsKey))
        {
            DateTime dateTime = DateTime.Parse(PlayerPrefs.GetString(playerPrefsKey));
            TimeSpan timeSpan = DateTime.Now - dateTime;
            OnUserBackOnline?.Invoke(timeSpan);
        }
    }*/
}
