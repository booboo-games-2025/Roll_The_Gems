using System;
using TMPro;
using UnityEngine;

public class OfflineEarning : MonoBehaviour
{
    private readonly string playerPrefsKey = "TimeWhenPaused";

    [SerializeField] private TMP_Text offlineForText, rewardText;
    [SerializeField] private GameObject welcomeBackUi;

    private void Start()
    {
        if(!PlayerPrefs.HasKey(playerPrefsKey)) return;
        
        DateTime dateTime = DateTime.Parse(PlayerPrefs.GetString(playerPrefsKey));
        TimeSpan timeSpan = DateTime.Now - dateTime;
        HandleOnUserBackOnline(timeSpan);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(playerPrefsKey, DateTime.Now.ToString());
    }
    
    private double moneyToAdd;
    public void HandleOnUserBackOnline(TimeSpan timeSpan)
    {
        if (timeSpan.Seconds < 60) return; // Less than a minute gets no reward
        
        welcomeBackUi.SetActive(true);
        int hours =  timeSpan.Hours;
        int minutes =  timeSpan.Minutes;
        offlineForText.text = "You`ve been away for " + hours + "h " + minutes + "m";
        
        // Give Money based on duration
        // Ref game does not add this reward towards Level progress
        int seconds = timeSpan.Seconds;
        if (hours >= 2)
        {
            seconds = 7200;
        }
        moneyToAdd = seconds / 60f * 50;
        rewardText.text = "<sprite=0> " + NumberFormatter.FormatNumberSmall(moneyToAdd);
    }
    
    public void ClaimOfflineEarning()
    {
        EconomyManager.instance.IncreaseEconomy(moneyToAdd);
        welcomeBackUi.SetActive(false);
    }
    
    // Rv Func
    public void ClaimOfflineEarning2X()
    {
        EconomyManager.instance.IncreaseEconomy(moneyToAdd * 2);
        welcomeBackUi.SetActive(false);
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
