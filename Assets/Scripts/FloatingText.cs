using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Show(string value, Color color)
    {
        text.text = "<sprite=0> " + value;
        text.color = color;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        text.color = Color.white;
        ObjectPooling.Instance.Release("float_text",gameObject);
    }
}
