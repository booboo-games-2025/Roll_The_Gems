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
    
    private void Awake()
    {
        instance = this;
        coinCount = GetDouble(MyConstants.COIN_COUNT, 50d);
        OnCoinChanged?.Invoke();
        UpdateCoinUi();
    }

    private void Start()
    {
        StartCoroutine(SaveEconomy());
    }

    IEnumerator SaveEconomy()
    {
        while (true)
        {
            SetDouble(MyConstants.COIN_COUNT, coinCount);
            yield return new WaitForSeconds(5);
        }
    }

    public void IncreaseEconomy(double coin)
    {
        coinCount += coin;
        //PlayerPrefs.SetInt(MyConstants.COIN_COUNT, coinCount);
        OnCoinChanged?.Invoke();
        UpdateCoinUi();
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
        coinText.text = NumberFormatter.FormatNumberSmall(coinCount);
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
