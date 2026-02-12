using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEvent clickEvent;
    
    private bool isPointerInside;
    internal Image image;

    private bool interactable;

    public bool Interactable
    {
        get => interactable;
        set
        {
            interactable = value;
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            image.raycastTarget = value;
        } 
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerInside = true;
        transform.DOKill();
        transform.DOScale(Vector3.one * 1.15f, 0.08f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.08f);
        }); 
        Vibration.VibratePop();
        AudioManager.instance.PlaySFX(SFXType.ButtonTap);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isPointerInside)
        clickEvent?.Invoke();
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}
