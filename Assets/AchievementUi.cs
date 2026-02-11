using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AchievementUi : MonoBehaviour
{
   [SerializeField] private TMP_Text toFromValueText;
   public AchievementType achievementType;
   public double maxValue;
   [SerializeField] private Image fillBar;
   public bool completed;
   public static Action OnAchievementComplete;

   public void UpdateProgress(double currentValue)
   {
      if(completed) return;
      
      if (achievementType == AchievementType.PlayGameTime)
      {
         toFromValueText.text = (int)(currentValue/60) + "m" + "/" + (int)(maxValue/60) + "m";
      }
      else
      {
         toFromValueText.text = NumberFormatter.FormatNumberSmall(currentValue) + "/" + NumberFormatter.FormatNumberSmall(maxValue);
      }
      fillBar.fillAmount = (float)(currentValue/maxValue);
      if (fillBar.fillAmount >= 1f)
      {
         toFromValueText.text = "Completed";
         OnAchievementComplete?.Invoke();
         //statusText.text = "Completed";
         fillBar.fillAmount = 1f;
         completed = true;
      }
   }
}
