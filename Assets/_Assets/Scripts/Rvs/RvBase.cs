using System;
using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public abstract class RvBase : MonoBehaviour
{
    [SerializeField] private float activeDuration;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject rvIcon;
    [SerializeField] private UiButton btn;
    
    public bool isActive;
    private Coroutine timerRoutine;
    [SerializeField] private Vector3 showPos, hidePos;

    private void Start()
    {
        timerText.text = activeDuration + " sec";
    }

    public void Activate()
    {
        if (isActive) return; // already active

        btn.Interactable = false;
        rvIcon.SetActive(true);
        timerText.gameObject.SetActive(true);
        isActive = true;

        OnEffectStart();                 // 🔹 Child-specific effect
        timerRoutine = StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        float elapsedTime = activeDuration;

        while (elapsedTime > 0)
        {
            UpdateTimer(elapsedTime);
            yield return new WaitForSeconds(1f);
            elapsedTime--;
        }

        EndBooster();
    }

    void EndBooster()
    {
        timerRoutine = null;
        rvIcon.SetActive(false);
        btn.Interactable = true;
        timerText.text = activeDuration +  " sec";
        isActive = false;
        ShowUi(false);

        OnEffectEnd();                   // 🔹 Child-specific cleanup
    }

    protected abstract void OnEffectStart();
    protected abstract void OnEffectEnd();

    void UpdateTimer(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    
    public void ShowUi(bool enable)
    {
        if (enable)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = hidePos;
            gameObject.SetActive(true);
            rectTransform.DOAnchorPos(showPos, 0.5f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
