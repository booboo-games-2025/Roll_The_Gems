using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public abstract class RvBase : MonoBehaviour
{
    [SerializeField] private float activeDuration;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject rvIcon;
    [SerializeField] private UiButton btn;

    private Coroutine timerRoutine;

    public void Activate()
    {
        if (timerRoutine != null) return; // already active

        btn.Interactable = false;
        rvIcon.SetActive(true);

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
        timerText.text = "2x Income";

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
}
