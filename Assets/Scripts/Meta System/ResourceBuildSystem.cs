using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResourceBuildSystem : MonoBehaviour
{
    [SerializeField] private GameObject _mainObject;
    [SerializeField] private GameObject _areasCanvas;
    [SerializeField] private List<BuildingSetup> _areas;
    [SerializeField] private Building _buildingPrefab;
    [SerializeField] private Camera _areasCamera;
    [SerializeField] private ObjectPooler _depositPooler;
    [SerializeField] private ParticleSystem _areaDoneVfx;

    [Header("Area UI")]
    [SerializeField] private string[] _areaNames;
    [SerializeField] private GameObject _areaCompleteTextContainer;
    [SerializeField] private TextMeshProUGUI _areaCompleteText;
    [SerializeField] private Button _nextButton;

    [Header("Requirement Curve")]
    [SerializeField] private int _baseValue;
    [SerializeField] private float _initialMultiplier;
    [SerializeField] private float _requirementIncreasePerBuilding;
    [SerializeField] private int _increaseConstant;

    [Header("UI")]
    [SerializeField] private Button _openButton;
    [SerializeField] private GameObject _availableIconObject;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _resourceCountText;
    [SerializeField] private TextMeshProUGUI _resourceCountTextHud;
    [SerializeField] private RectTransform _depositButtonTransform;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private WorldToUIAnchor _worldToUIAnchor;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private GameObject _progressInfoUI;
    [SerializeField] private List<GameObject> _allowedInputOverObjects;

    [Header("Save")]
    [SerializeField] private string _saveKeyPrefix = "resource_build_system";

    [Header("Animation Parameters")]
    [SerializeField] private float _resourcesPerSecond;

    public event Action<double> OnResourceChanged;
    public event Action OnBuildingCompleted;
    public event Action<int> OnAreaCompleted;

    private List<Building> _currentBuildingsList = new List<Building>();
    private BuildingSetup _currentArea;
    private Building _currentBuildingInProgress;
    private CameraFocus _cameraFocusComponent;

    private int _currentAreaIndex;
    private int _currentBuildingIndex;
    private double _currentProgress;
    private int _totalBuildingsDone;
    private int _totalAreasDone;

    private double _totalResources;
    private double _depositedThisSession;
    private double _availableThisSession;

    private bool _isCurrentBuildingRequirementMet;
    private Coroutine _depositCoroutine;

    private string ResourceKey => $"{_saveKeyPrefix}_resource";
    private string AreaIndexKey => $"{_saveKeyPrefix}_area_index";
    private string BuildingInProgressKey => $"{_saveKeyPrefix}_building_in_progress";
    private string BuildingProgressKey => $"{_saveKeyPrefix}_building_progress";
    private string AreasDoneKey => $"{_saveKeyPrefix}_areas_done";

    private void Awake()
    {
        _cameraFocusComponent = _areasCamera.GetComponent<CameraFocus>();

        _totalResources = PlayerPrefsExtension.GetDouble(ResourceKey, 0);
        _currentAreaIndex = PlayerPrefs.GetInt(AreaIndexKey, 0);
        _currentBuildingIndex = PlayerPrefs.GetInt(BuildingInProgressKey, 0);
        _currentProgress = PlayerPrefsExtension.GetDouble(BuildingProgressKey, 0);
        _totalAreasDone = PlayerPrefs.GetInt(AreasDoneKey, 0);

        for (int i = 0; i < _totalAreasDone; i++)
        {
            int areaIndex = i % _areas.Count;
            _totalBuildingsDone += _areas[areaIndex].GetNumberOfBuildings();
        }

        _resourceCountText.text = _resourceCountTextHud.text = NumberFormatter.FormatNumberSmall(_totalResources);
        GenerateCurrentArea();
    }

    private void Start()
    {
        _openButton.onClick.AddListener(Show);
        _closeButton.onClick.AddListener(Hide);
        _nextButton.onClick.AddListener(() =>
        {
            _areaCompleteTextContainer.SetActive(false);
            GenerateCurrentArea();
        });

        _availableIconObject.SetActive(_totalResources > 0);

        InputManager.OnTouchPressed += HandleOnTouchPressed;
        InputManager.OnTouchReleased += HandleOnTouchReleased;
    }

    private void Update()
    {
        if (_currentBuildingInProgress == null) return; // area complete, nothing to show progress for

        float progress = _currentBuildingInProgress.GetProgress();
        _progressBar.fillAmount = progress;
        _progressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
    }

    private void HandleOnTouchReleased()
    {
        EndDeposit();
    }

    private void HandleOnTouchPressed(GameObject pUiObject)
    {
        if(!_allowedInputOverObjects.Contains(pUiObject)) return;

        BeginDeposit();
    }

    private void GenerateCurrentArea()
    {
        _currentArea = _areas[_currentAreaIndex];

        foreach (var area in _areas) area.gameObject.SetActive(false);
        _currentArea.gameObject.SetActive(true);

        List<Transform> buildingPositionRefs = _currentArea.GetBuildingPositionRefs();

        for (int i = 0; i < buildingPositionRefs.Count; i++)
        {
            Transform positionRef = buildingPositionRefs[i];
            Building building = Instantiate(_buildingPrefab, positionRef.position, positionRef.rotation, _currentArea.transform);

            bool isCurrent = i == _currentBuildingIndex;
            int globalIndex = i + _totalBuildingsDone;
            double requirement = Math.Round(_baseValue * (_initialMultiplier + globalIndex * _requirementIncreasePerBuilding) + (_increaseConstant * globalIndex * globalIndex), 0);

            building.SetRequirement(requirement, isCurrent ? _currentProgress : 0);

            SpriteRenderer refSprite = positionRef.GetComponent<SpriteRenderer>();
            building._BuildingVisual.sprite = refSprite.sprite;
            building._BuildingVisual.sortingOrder = refSprite.sortingOrder;

            building.ToggleActiveness(isCurrent);

            _currentBuildingsList.Add(building);
        }

        _totalBuildingsDone += _currentBuildingIndex;

        for (int i = 0; i < _currentBuildingIndex; i++)
        {
            _currentBuildingsList[i].SetAsDone();
        }

        _currentBuildingInProgress = _currentBuildingsList[_currentBuildingIndex];
        _isCurrentBuildingRequirementMet = false;

        ToggleProgressInfoUI(true);
    }

    private IEnumerator HandleDeposit()
    {
        double requirement = _currentBuildingInProgress.GetCurrentRequirement();

        _worldToUIAnchor.SetTarget(_currentBuildingInProgress.transform);
        _cameraFocusComponent.FocusOn(_currentBuildingInProgress.transform.position);

        float accumulatedResources = 0f;

        while (_depositedThisSession < requirement && _totalResources > 0)
        {
            accumulatedResources += _resourcesPerSecond * Time.deltaTime;

            int resourcesToDeposit = Mathf.FloorToInt(accumulatedResources);
            accumulatedResources -= resourcesToDeposit;

            for (int i = 0; i < resourcesToDeposit; i++)
            {
                if (_depositedThisSession >= requirement || _totalResources <= 0) break;

                DepositOne();
            }

            yield return null;
        }

        OnDepositSessionDone();

        if (_depositedThisSession == requirement)
        {
            _isCurrentBuildingRequirementMet = true;

            _currentBuildingIndex += 1;
            PlayerPrefs.SetInt(BuildingInProgressKey, _currentBuildingIndex);
            _totalBuildingsDone += 1;
        }

        _depositedThisSession = 0;
    }

    private void DepositOne()
    {
        Vector2 buttonScreen = RectTransformUtility.WorldToScreenPoint(_areasCamera, _depositButtonTransform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, buttonScreen, _areasCamera, out Vector2 buttonLocal);

        DepositedResource unit = _depositPooler.GetPooledObject().GetComponent<DepositedResource>();
        Building targetBuilding = _currentBuildingInProgress;

        unit.Play(buttonLocal, _worldToUIAnchor.GetComponent<RectTransform>(),
            pOnComplete: () => HandleUnitReachedBuilding(unit.gameObject, targetBuilding));

        _depositedThisSession += 1;
        _availableThisSession -= 1;
        _totalResources -= 1;
        _currentProgress += 1;

        PlayerPrefsExtension.SetDouble(ResourceKey, _totalResources);
        _resourceCountText.text = _resourceCountTextHud.text = NumberFormatter.FormatNumberSmall(_totalResources);
        OnResourceChanged?.Invoke(_totalResources);
    }

    private void HandleUnitReachedBuilding(GameObject unitObject, Building targetBuilding)
    {
        _depositPooler.Release(unitObject);

        if (targetBuilding == null) return;

        targetBuilding.AddBrick();

        if (targetBuilding.IsDone())
        {
            bool areaDone = CheckIfAreaDone();

            if (!areaDone)
            {
                GetToNewBuilding();
            }
            else
            {
                _currentProgress = 0;
            }

            OnBuildingCompleted?.Invoke();
        }
    }

    private void GetToNewBuilding()
    {
        if (_currentBuildingsList.Count > _currentBuildingIndex)
        {
            _currentBuildingInProgress.SetAsDone();
            _currentBuildingInProgress = _currentBuildingsList[_currentBuildingIndex];
            _currentBuildingInProgress.ToggleActiveness(true);
        }

        _currentProgress = 0;
        _isCurrentBuildingRequirementMet = false;
    }

    private bool CheckIfAreaDone()
    {
        if (_currentBuildingIndex != _currentArea.GetNumberOfBuildings()) return false;

        string areaName = (_areaNames != null && _areaNames.Length > 0)
            ? _areaNames[_currentAreaIndex % _areaNames.Length]
            : $"Area {_currentAreaIndex + 1}";

        _areaCompleteText.text = areaName;
        _areaCompleteTextContainer.SetActive(true);

        int completedAreaIndex = _currentAreaIndex;

        _currentAreaIndex = (_currentAreaIndex + 1) % _areas.Count;
        _totalAreasDone += 1;
        _currentBuildingIndex = 0;

        PlayerPrefs.SetInt(AreasDoneKey, _totalAreasDone);
        PlayerPrefs.SetInt(AreaIndexKey, _currentAreaIndex);
        PlayerPrefs.SetInt(BuildingInProgressKey, 0);

        _currentBuildingInProgress.SetAsDone();
        _currentBuildingInProgress = null;
        _isCurrentBuildingRequirementMet = false;

        _currentBuildingsList = new List<Building>();
        _areaDoneVfx.Play();
        ToggleProgressInfoUI(false);

        OnAreaCompleted?.Invoke(completedAreaIndex);

        return true;
    }

    private void OnDepositSessionDone()
    {
        _cameraFocusComponent.MoveToOriginalPosition();

        double resource = _totalResources;
        _availableIconObject.SetActive(resource > 0);
        if (resource <= 0) ToggleProgressInfoUI(false);

        _depositCoroutine = null;
    }

    private void Show()
    {
        ToggleProgressInfoUI(_totalResources > 0);
        _mainObject.SetActive(true);
        _areasCanvas.SetActive(true);

        _resourceCountText.text = _resourceCountTextHud.text = NumberFormatter.FormatNumberSmall(_totalResources);
    }

    private void Hide()
    {
        _mainObject.SetActive(false);
        _areasCanvas.SetActive(false);
    }

    private void ToggleProgressInfoUI(bool active) => _progressInfoUI.SetActive(active);

    private void OnDisable()
    {
        PlayerPrefsExtension.SetDouble(BuildingProgressKey, _currentProgress);
        PlayerPrefsExtension.SetDouble(ResourceKey, _totalResources);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        InputManager.OnTouchPressed -= HandleOnTouchPressed;
        InputManager.OnTouchReleased -= HandleOnTouchReleased;
    }

    public void AddResource(double amount)
    {
        if (amount <= 0) return;

        _totalResources += amount;
        PlayerPrefsExtension.SetDouble(ResourceKey, _totalResources);

        _availableIconObject.SetActive(true);
        OnResourceChanged?.Invoke(_totalResources);
        _resourceCountText.text = _resourceCountTextHud.text = NumberFormatter.FormatNumberSmall(_totalResources);
    }

    public double GetCurrentResource() => _totalResources;

    public void BeginDeposit()
    {
        if (!_mainObject.activeSelf || _depositCoroutine != null) return;
        if (_currentBuildingInProgress == null) return;
        if (_isCurrentBuildingRequirementMet) return;

        _availableThisSession = _totalResources;
        if (_availableThisSession <= 0) return;

        _depositCoroutine = StartCoroutine(HandleDeposit());
    }

    public void EndDeposit()
    {
        if (!_mainObject.activeSelf) return;

        if (_depositCoroutine != null) StopCoroutine(_depositCoroutine);

        OnDepositSessionDone();
        _depositedThisSession = 0;
    }
}