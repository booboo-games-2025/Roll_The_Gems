using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class HoleSkinsManager : MonoBehaviour
{
    public static HoleSkinsManager instance;

    public static Action OnSkinChanged;
    public static int currentSkinIndex;

    [SerializeField] private GameObject skinsContainer;
    [SerializeField] private Transform panel;
    [SerializeField] private SkinButton[] skinButtonsList;
    public Sprite selectedBg, unselectedBg;

    private void Awake()
    {
        instance = this;
    }
    
    Coroutine skinCoroutine;
    public void SetGemsSkin(int index)
    {
        if (skinCoroutine != null)
        {
            StopCoroutine(skinCoroutine);
            skinCoroutine = null;
        }
        currentSkinIndex = index;
        OnSkinChanged?.Invoke();
        UpdateModesUi();
        skinCoroutine = StartCoroutine(ResetGemsSkin());
    }

    IEnumerator ResetGemsSkin()
    {
        float elaspedTime = 180;
        while (elaspedTime > 0f)
        {
            skinButtonsList[currentSkinIndex].UpdateTimer(elaspedTime);
            yield return new WaitForSeconds(1);
            elaspedTime--;
        }
        currentSkinIndex = 0;
        OnSkinChanged?.Invoke();
        UpdateModesUi();
    }

    public void UpdateModesUi()
    {
        for (int i = 0; i < skinButtonsList.Length; i++)
        {
            if (currentSkinIndex == i)
            {
                skinButtonsList[i].state = SkinButtonState.Selected;
                skinButtonsList[i].ChangeUiAccordingToState();
                continue;
            }
            skinButtonsList[i].state = SkinButtonState.Lock;
            skinButtonsList[i].ChangeUiAccordingToState();
        }
    }

    public void Show()
    {
        panel.DOKill();
        panel.transform.localScale = Vector3.zero;
        skinsContainer.SetActive(true);
        panel.transform.DOScale(Vector3.one, 0.25f);
    }

    public void Hide()
    {
        panel.DOKill();
        panel.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            skinsContainer.SetActive(false);
        });
    }
    
}
