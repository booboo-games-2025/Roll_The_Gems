using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class HoleSkinsManager : MonoBehaviour
{
    public static HoleSkinsManager instance;
    
    [SerializeField] private GameObject skinsContainer;
    [SerializeField] private Transform panel;

    private void Awake()
    {
        instance = this;
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
