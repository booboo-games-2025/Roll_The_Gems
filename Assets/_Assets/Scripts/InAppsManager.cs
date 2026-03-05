using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InAppsManager : MonoBehaviour
{
    public static InAppsManager Instance;
    public InAppManagerUnityIAP_Boombit baseInAppsManager;
    //public InAppManagerUnityIAPNative baseInAppsManager;

    public bool removeAdsPurchase = false;
    public bool isRestoed;

    // Use this for initialization
    void Awake()
    {
        if (Instance != null)
        {
            print("Instance Already Exist: Destroying this gameobject");
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializeInApps();
    }

    public void InitializeInApps()
    {
#if UNITY_ANDROID
        baseInAppsManager.InitializeInApps();
#endif
    }

    /*void InAppChecker()
    {
        removeAdsPurchase = false;
        if (!PlayerPrefs.HasKey(Constants.REMOVE_AD))
        {
            PlayerPrefs.SetInt(Constants.REMOVE_AD, 0);
        }


        if (PlayerPrefs.GetInt(Constants.REMOVE_AD) == 1)
        {
            removeAdsPurchase = true;
        }
    }

    public void OnRemoveAd_Purchase_Success()
    {
        HCSDKManager.INSTANCE.HideBanner();
        PlayerPrefs.SetInt(Constants.REMOVE_AD, 1);
        print("Remove ads Purchase Success");
        removeAdsPurchase = true;
    }

    public void OnPremiumPack_Purchase_Success()
    {
#if !UNITY_EDITOR
        HCSDKManager.INSTANCE.HideBanner();
#endif
        PlayerPrefs.SetInt(Constants.REMOVE_AD, 1);
        removeAdsPurchase = true;

        LevelManager.instance.Token += 50;
        LevelManager.instance.Gems += 50;
        LevelManager.instance.Balance += 10000;

        //Skin no.4 & 10 is premium outfit
        SkinsManager.instance.SkinUnlock(4);
        SkinsManager.instance.SkinUnlock(10);

        TutorialManager.instance.player.HoverBoardOn = true;

        //if (TutorialManager.instance.player.Male)
        //    SkinsManager.instance.ApplySkin(4);
        //else
        //    SkinsManager.instance.ApplySkin(10);

        RVManager.instance.RemoveHoverboardRVAfterIAP();

        PlayerPrefs.SetInt(Constants.PREMIUM_PACK, 1);
        print("Premium Pack Purchase Success");
    }

    //public void PurchaseCash(int index)
    //{
    //    Debug.Log("InApp Purchase Cash index: " + index);
    //    switch(index)
    //    {
    //        case 0: baseInAppsManager.CashStack();
    //            break;
    //        case 1:
    //            baseInAppsManager.CashBundle();
    //            break;
    //        case 2:
    //            baseInAppsManager.CashHeap();
    //            break;
    //    }
    //}

    public void PurchaseGems(int index)
    {
        Debug.Log("InApp Purchase Gems index: " + index);
        switch (index)
        {
            case 0:
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.GemPocket();
                break;
            case 1:
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");       
                baseInAppsManager.GemStash();
                break;
            case 2:
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.GemJackpot();
                break;
        }
    }

    public void PurchaseToken(int index)
    {
        Debug.Log("InApp Purchase Token index: " + index);
        switch (index)
        {
            case 0:
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.TokenBox();
                break;
            case 1:
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.TokenVault();
                break;
        }
    }
    public void ProPass_Purchase_Success()
    {
        PlayerPrefs.SetInt(Constants.PROPASS, 1);
        print("Pro Pass Purchase Success");
    }

    public void ElitePass_Purchase_Success()
    {
        PlayerPrefs.SetInt(Constants.ELITEPASS, 1);
        print("Elite Pass Purchase Success");
    }

    public void RestorePurchases()
    {
        baseInAppsManager.RestorePurchases();
    }


    //Should be attached to Buttons
    #region InApp Process

    public void RemoveAdsInApp()
    {
        if (PlayerPrefs.GetInt(Constants.REMOVE_AD) == 0)
        {
            if (!GeneralManager.CheckInternetConnection())
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Alert, Constants.NO_INTERNET_CONNECTION, "ok");
            }
            else
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.RemoveAds();
            }
        }
        else
        {
            GenericPopUp.Instance.ShowPopUp(PopUpType.Alert,
                "Already Purchased" + "\nYou already purchased Remove Ad.\n", "ok");
        }
    }

    public void PremiumPackInApp()
    {
        if (PlayerPrefs.GetInt(Constants.PREMIUM_PACK) == 0)
        {
            if (!GeneralManager.CheckInternetConnection())
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Alert, Constants.NO_INTERNET_CONNECTION, "ok");
            }
            else
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.PremiumPack();
            }
        }
        else
        {
            GenericPopUp.Instance.ShowPopUp(PopUpType.Alert,
                "Already Purchased" + "\nYou already purchased Premium Pack.\n", "ok");
        }
    }

    public void ProPassIAP()
    {
        if (PlayerPrefs.GetInt(Constants.PROPASS) == 0)
        {
            if (!GeneralManager.CheckInternetConnection())
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Alert, Constants.NO_INTERNET_CONNECTION, "ok");
            }
            else
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.ProPass();
            }
        }
        else
        {
            GenericPopUp.Instance.ShowPopUp(PopUpType.Alert,
                "Already Purchased" + "\nYou already purchased Pro Pass.\n", "ok");
        }
    }

    public void ElitePassIAP()
    {
        if (PlayerPrefs.GetInt(Constants.ELITEPASS) == 0)
        {
            if (!GeneralManager.CheckInternetConnection())
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Alert, Constants.NO_INTERNET_CONNECTION, "ok");
            }
            else
            {
                GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
                baseInAppsManager.ElitePass();
            }
        }
        else
        {
            GenericPopUp.Instance.ShowPopUp(PopUpType.Alert,
                "Already Purchased" + "\nYou already purchased Elite Pass.\n", "ok");
        }
    }


    public void RestoreInApp()
    {
        if (!GeneralManager.CheckInternetConnection())
        {
            GenericPopUp.Instance.ShowPopUp(PopUpType.Alert, Constants.NO_INTERNET_CONNECTION, "ok");
        }
        else
        {
            RestorePurchases();
            GenericPopUp.Instance.ShowPopUp(PopUpType.Blank, Constants.PLEASE_WAIT, "ok");
        }
    }*/
}
