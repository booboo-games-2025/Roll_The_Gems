using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] Image bgImage;
    [SerializeField] TMP_Text onoffText;
    [SerializeField] Sprite onSprite, offSprite;
    [SerializeField] string onText, offText;

    public void UpdateUi(bool state)
    {
        bgImage.sprite = state ? onSprite : offSprite;
        onoffText.text = state ? onText : offText;
    }
}
