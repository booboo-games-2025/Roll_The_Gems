using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RvManager : MonoBehaviour
{
    public RvBase[] allRvs;
    private int incomeRvIndex;

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
}
