using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI descriptionTxt;
    public TextMeshProUGUI btnTxt;

    PopUpType type;
    string title;
    string description;
    string btnTitle;

    public string Description 
    { 
        get => description;
        set
        {
            description = value;
            descriptionTxt.text = description;
        }
    }
    public string BtnTitle 
    { 
        get => btnTitle;
        set
        {
            btnTitle = value;
            btnTxt.text = btnTitle;
        }
    }

    public PopUpType Type 
    { 
        get => type;
        set
        {
            type = value;

            if (type == PopUpType.Blank)
            {
                //titleTxt.gameObject.SetActive(false);
                btnTxt.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                //titleTxt.gameObject.SetActive(true);
                btnTxt.transform.parent.gameObject.SetActive(true);
            }
        }
    }
}
