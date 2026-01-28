using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUi : MonoBehaviour
{
   [SerializeField] private TMP_Text toFromValueText, statusText;
   public AchievementType achievementType;
   public double maxValue;
   [SerializeField] private Image fillBar;
   public bool completed;

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
         statusText.text = "Completed";
         fillBar.fillAmount = 1f;
         completed = true;
      }
   }
}
