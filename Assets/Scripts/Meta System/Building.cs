using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private const string PROGRESS_SHADER_KEY = "_Progress";

    public SpriteRenderer _BuildingVisual;
    public GameObject _BuildingPanel;
    public TextMeshProUGUI _RequirementText;
    public double _TotalRequirement;
    public RectTransform _RequirementPanel;
    public float _BottomPosition;
    public float _TopPosition;

    [SerializeField] private ParticleSystem _builtVfx;
    private float _CurrentProgress;
    private double _CurrentRequirement;

    private void OnEnable()
    {
        _RequirementText.text = NumberFormatter.FormatNumberSmall(_CurrentRequirement);
    }

    private void UpdateProgress()
    {
        _CurrentProgress = 1f - (float)(_CurrentRequirement / _TotalRequirement);

        Vector2 position = _RequirementPanel.anchoredPosition;
        position.y = Mathf.Lerp(_BottomPosition, _TopPosition, _CurrentProgress);
        _RequirementPanel.anchoredPosition = position;
        _BuildingVisual.SetFloat(PROGRESS_SHADER_KEY, _CurrentProgress);
    }

    public void ToggleActiveness(bool pIsActive)
    {
        _BuildingPanel.SetActive(pIsActive);
        _BuildingVisual.enabled = pIsActive;
    }

    public void AddBrick()
    {
        _CurrentRequirement -= 1;

        _RequirementText.text = _CurrentRequirement.ToString();

        UpdateProgress();
    }

    public void SetAsDone()
    {
        _BuildingVisual.enabled = true;
        _BuildingPanel.SetActive(false);
        _CurrentRequirement = 0;
        _builtVfx.Play();

        UpdateProgress();
    }

    public void SetRequirement(double pRequirement, double pCurrentProgress)
    {
        _TotalRequirement = pRequirement;
        _CurrentRequirement = _TotalRequirement - pCurrentProgress;

        _RequirementText.text = NumberFormatter.FormatNumberSmall(_CurrentRequirement);

        UpdateProgress();
    }

    public double GetCurrentRequirement() 
    { 
        return _CurrentRequirement; 
    }

    public bool IsDone()
    {
        return _CurrentRequirement == 0;
    }

    public float GetProgress()
    {
        return _CurrentProgress;
    }
}
