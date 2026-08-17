using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;

    [SerializeField] private TMP_Text coinText;

    public double coinCount;
    public static Action OnCoinChanged;
    public double permanentCoinCount;

    [Header("Meta Progression")]
    [SerializeField] private ResourceBuildSystem _resourceBuildSystem;
    [SerializeField] private double _metaResourceEconomyMultiplier;
    
    private void Awake()
    {
        instance = this;
        coinCount = GetDouble(MyConstants.COIN_COUNT, 0);
        permanentCoinCount = GetDouble(MyConstants.PERMANENT_COIN_COUNT, 0);
        OnCoinChanged?.Invoke();
        UpdateCoinUi();
    }

    private void Start()
    {
        StartCoroutine(SaveEconomyRoutine());
    }

    private IEnumerator SaveEconomyRoutine()
    {
        while (true)
        {
            SaveEconomy();
            yield return new WaitForSeconds(5);
        }
    }

    private void AddMetaResources(double pCoins)
    {
        double metaResourceCount = Math.Round(pCoins * _metaResourceEconomyMultiplier, 0);
        _resourceBuildSystem.AddResource(metaResourceCount);
    }

    public void SaveEconomy()
    {
        if (PlayerPrefs.GetInt(MyConstants.StartFtueCompleted, 0) == 1)
        {
            SetDouble(MyConstants.COIN_COUNT, coinCount);
            SetDouble(MyConstants.PERMANENT_COIN_COUNT, permanentCoinCount);
        }
    }

    public void IncreaseEconomy(double pCoinAmount)
    {
        coinCount += pCoinAmount;
        permanentCoinCount += pCoinAmount;
        //PlayerPrefs.SetInt(MyConstants.COIN_COUNT, coinCount);
        OnCoinChanged?.Invoke();
        UpdateCoinUi();

        AddMetaResources(pCoinAmount);
        Achievements.OnAchievementsUpdated?.Invoke(pCoinAmount,AchievementType.EarnCoin);
    }

    public void DecreaseEconomy(double coin)
    {
        coinCount -= coin;
        if (coinCount < 0)
        {
            coinCount = 0;
        }
        //PlayerPrefs.SetInt(MyConstants.COIN_COUNT, coinCount);
        OnCoinChanged?.Invoke();
        UpdateCoinUi();
    }
    
    void UpdateCoinUi()
    {
        coinText.text = "<sprite=0> " + NumberFormatter.FormatNumberSmall(coinCount);
    }
    
    public static void SetDouble(string key, double value)
    {
        PlayerPrefs.SetString(key, value.ToString("R", CultureInfo.InvariantCulture));
    }

    public static double GetDouble(string key, double defaultValue = 0d)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        var s = PlayerPrefs.GetString(key, "0");
        if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            return result;

        return defaultValue;
    }
}
