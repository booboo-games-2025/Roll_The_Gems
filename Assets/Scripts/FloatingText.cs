using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Show(string value)
    {
        text.text = value;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        ObjectPooling.Instance.Release("float_text",gameObject);
    }
}
