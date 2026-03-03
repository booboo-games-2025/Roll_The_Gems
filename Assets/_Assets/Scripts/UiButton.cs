using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class UiButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public UnityEvent clickEvent;
    
    [Header("Mode")]
    [SerializeField] private bool isHoldable = false;

    [Header("Hold Settings")]
    [SerializeField] private float startInterval = 0.4f;
    [SerializeField] private float minInterval = 0.06f;
    [SerializeField] private float acceleration = 0.02f;
    
    private bool isPointerInside;
    private bool isHolding;
    private Coroutine holdCoroutine;
    
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
            
            if (!value)
            {
                // Stop everything immediately
                isPointerInside = false;
                StopHolding();
            }
        } 
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerInside = true;
        // transform.DOKill();
        // transform.DOScale(Vector3.one * 1.15f, 0.06f).OnComplete(() =>
        // {
        //     transform.DOScale(Vector3.one, 0.06f);
        // }); 
        // Vibration.VibratePop();
        // AudioManager.instance.PlaySFX(SFXType.ButtonTap);
        
        if (isHoldable)
        {
            isHolding = true;
            // First click instantly
            TriggerClick();
            holdCoroutine = StartCoroutine(HoldClickRoutine());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        StopHolding();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isHoldable)
        {
            // Normal tap button
            if (isPointerInside)
                TriggerClick();
        }
        StopHolding();
    }
    
    private void StopHolding()
    {
        isHolding = false;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
    }

    private IEnumerator HoldClickRoutine()
    {
        float currentInterval = startInterval;
        yield return new WaitForSeconds(currentInterval);
        while (isHolding && isPointerInside)
        {
            TriggerClick();
            yield return new WaitForSeconds(currentInterval);
            currentInterval = Mathf.Max(currentInterval - acceleration, minInterval);
        }
    }
    
    private void TriggerClick()
    {
        // Invoke event
        clickEvent?.Invoke();
        // Scale animation
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOScale(Vector3.one * 1.15f, 0.08f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 0.08f);
        });
        // Sound
        AudioManager.instance.PlaySFX(SFXType.ButtonTap);
        // Haptic
        Vibration.VibratePop();
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}
