using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using System.Collections.Generic;

public class RvManager : MonoBehaviour
{
    public static RvManager instance;
    
    public List<RvBase> allRvs;
    private int incomeRvIndex;
    [SerializeField] private RectTransform floatingRv;
    [SerializeField] TMP_Text rewardText;
    [SerializeField] private GameObject floatingRvPanel;
    Coroutine floatingRvCoroutine;

    private void Awake()
    {
        instance = this;
        floatingRvCoroutine = StartCoroutine(FloatingRvTimer());
    }

    // start Showing Rv Alternative
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(30);
        while (true)
        {
            // Show Income Rv
            SwitchIncomeRvs();
            yield return new WaitForSeconds(30); // wait 30 sec
            DisableRvs(); // Hide Income Rv
            yield return new WaitForSeconds(5);
        }
    }

    bool IsRvRunning()
    {
        bool isRvRunning = false;
        for (int i = 0; i < allRvs.Count; i++)
        {
            if (allRvs[i].isActive)
            {
                isRvRunning = true;
                break;
            }
        }
        return isRvRunning;
    }
    
    void SwitchIncomeRvs()
    {
        if (!allRvs[incomeRvIndex].isActive)
        {
            //DisableAllRvs();
            allRvs[incomeRvIndex].ShowUi(true);
            incomeRvIndex++;
            if (incomeRvIndex >= allRvs.Count)
            {
                incomeRvIndex = 0;
            }
        }
    }

    void DisableRvs()
    {
        if (!IsRvRunning())
        {
            DisableAllRvs();
        }
    }

    void DisableAllRvs()
    {
        for (int i = 0; i < allRvs.Count; i++)
        {
            allRvs[i].gameObject.SetActive(false);
        }
    }

    public void ClearRv(int rvIndex)
    {
        if (allRvs[rvIndex].isActive)
        {
            allRvs[rvIndex].EndBooster();
        }
        allRvs.RemoveAt(rvIndex);
    }
    
    #region FloatingRv

    private double moneyRandom;
    IEnumerator FloatingRvTimer()
    {
        yield return new WaitForSeconds(120f);
        moneyRandom = MathF.Pow(Random.Range(16,22) * PlayerPrefs.GetInt(MyConstants.RING_LEVEL), 1.5f);
        rewardText.text = NumberFormatter.FormatNumberSmall(moneyRandom);
        floatingRv.gameObject.SetActive(true);
        floatingRv.anchoredPosition = new Vector2(Random.Range(-450, 450), 700);
        yield return new WaitForSeconds(15f);
        floatingRv.gameObject.SetActive(false);
        floatingRvCoroutine = null;
    }

    
    public TMP_Text moneyText;
    public void ClickedOnFloatingRv()
    {
        floatingRvPanel.SetActive(true);
        floatingRv.gameObject.SetActive(false);
        if (floatingRvCoroutine != null)
        {
            StopCoroutine(floatingRvCoroutine);
            floatingRvCoroutine = null;
        }
        moneyText.text = "<Sprite=0> " + NumberFormatter.FormatNumberSmall(moneyRandom);
    }

    public void ClaimFloatingRv()
    {
        floatingRvPanel.SetActive(false);
        EconomyManager.instance.IncreaseEconomy(moneyRandom);
        floatingRvCoroutine = StartCoroutine(FloatingRvTimer());
        AudioManager.instance.PlaySFX(SFXType.Claim);
    }

    public void CancelRv()
    {
        floatingRvPanel.SetActive(false);
        floatingRvCoroutine = StartCoroutine(FloatingRvTimer());
    }

    #endregion
}
