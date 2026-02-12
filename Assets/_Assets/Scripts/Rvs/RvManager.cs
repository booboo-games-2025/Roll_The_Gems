using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class RvManager : MonoBehaviour
{
    public RvBase[] allRvs;
    private int incomeRvIndex;
    [SerializeField] private RectTransform floatingRv;
    [SerializeField] private GameObject floatingRvPanel;
    Coroutine floatingRvCoroutine;

    private void Awake()
    {
        floatingRvCoroutine = StartCoroutine(FloatingRvTimer());
    }

    // start Showing Rv Alternative
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(20);
        while (true)
        {
            // Show Income Rv
            SwitchIncomeRvs();
            yield return new WaitForSeconds(30); // wait 30 sec
            DisableIncomeRvs(); // Hide Income Rv
            yield return new WaitForSeconds(5);
        }
    }

    bool IsRvRunning()
    {
        bool isRvRunning = false;
        for (int i = 0; i < allRvs.Length; i++)
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
        if (!IsRvRunning())
        {
            DisableAllRvs();
            allRvs[incomeRvIndex].ShowUi(true);
            incomeRvIndex++;
            if (incomeRvIndex == allRvs.Length)
            {
                incomeRvIndex = 0;
            }
        }
    }

    void DisableIncomeRvs()
    {
        if (!IsRvRunning())
        {
            DisableAllRvs();
        }
    }

    void DisableAllRvs()
    {
        for (int i = 0; i < allRvs.Length; i++)
        {
            allRvs[i].gameObject.SetActive(false);
        }
    }
    
    #region FloatingRv

    IEnumerator FloatingRvTimer()
    {
        yield return new WaitForSeconds(120f);
        floatingRv.gameObject.SetActive(true);
        floatingRv.anchoredPosition = new Vector2(Random.Range(-450, 450), 700);
        yield return new WaitForSeconds(15f);
        floatingRv.gameObject.SetActive(false);
        floatingRvCoroutine = null;
    }

    private float moneyRandom;
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
        moneyRandom = Random.Range(100, 1000);
        moneyText.text = "<Sprite=0> " + moneyRandom;
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
