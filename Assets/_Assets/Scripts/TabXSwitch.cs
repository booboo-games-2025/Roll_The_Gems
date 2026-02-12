using UnityEngine;
using UnityEngine.UI;

public class TabXSwitch : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite seletecSprite, deselectedSprite;
    public UiButton uiButton;

    public void SwitchSprite(bool flag)
    {
        img.sprite = flag ? seletecSprite : deselectedSprite;
        uiButton.Interactable = !flag;
    }
}
