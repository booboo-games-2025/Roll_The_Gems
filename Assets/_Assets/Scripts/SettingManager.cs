using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private Transform settingPanel;
    [SerializeField] private GameObject mainMenuButton, replayButton;
    [SerializeField] private ButtonToggle soundToggle, musicToggle, hapticToggle;

    public static Action<bool> OnSettingPanelOpen;
    
    public static bool sound = true, music = true, haptic = true;
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //CloseSettingPanel();
        //AdjustUi();
    }
    
    void AdjustUi()
    {
        /*if (SceneManager.GetActiveScene().buildIndex > MyConstants.MAIN_MENU_INDEX)
        {
            mainMenuButton.SetActive(true);
            replayButton.SetActive(true);
            soundToggle.transform.localPosition = new Vector2(soundToggle.transform.localPosition.x, 110);
            musicToggle.transform.localPosition = new Vector2(musicToggle.transform.localPosition.x, 110);
            hapticToggle.transform.localPosition = new Vector2(hapticToggle.transform.localPosition.x, 110);
            settingPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(620,500);
        }
        else
        {
            mainMenuButton.SetActive(false);
            replayButton.SetActive(false);
            soundToggle.transform.localPosition = new Vector2(soundToggle.transform.localPosition.x, 30);
            musicToggle.transform.localPosition = new Vector2(musicToggle.transform.localPosition.x, 30);
            hapticToggle.transform.localPosition = new Vector2(hapticToggle.transform.localPosition.x, 30);
            settingPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(620,380);
        }*/
    }
    
    
    private void Awake()
    {
        instance = this;
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void Start()
    {
        //Vibration.Init();
        UpdateInStart();
        //HCSDKManager.INSTANCE.ShowBanner();
    }

    public void OpenSettingPanel()
    {
        OnSettingPanelOpen?.Invoke(true);
        settingPanel.DOKill();
        settingPanel.transform.localScale = Vector3.zero;
        settingMenu.SetActive(true);
        settingPanel.transform.DOScale(Vector3.one, 0.25f);
    }

    public void CloseSettingPanel()
    {
        OnSettingPanelOpen?.Invoke(false);
        settingPanel.DOKill();
        settingPanel.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            settingMenu.SetActive(false);
        });
    }

    void UpdateInStart()
    {
        //AudioManager.instance.ToogleSound(sound);
       // AudioManager.instance.ToogleMusic(music);
       // Vibration.hapticsEnabled = haptic;
        soundToggle.UpdateUi(sound);
        musicToggle.UpdateUi(music);
        hapticToggle.UpdateUi(haptic);
    }

    public void ToogleSound()
    {
        sound = !sound;
       // AudioManager.instance.ToogleSound(sound);
        soundToggle.UpdateUi(sound);
    }

    public void ToogleMusic()
    {
        music = !music;
       // AudioManager.instance.ToogleMusic(music);
        musicToggle.UpdateUi(music);
    }

    public void ToogleHaptic()
    {
        haptic = !haptic;
        //Vibration.hapticsEnabled = haptic;
        hapticToggle.UpdateUi(haptic);
    }

    public void Replay()
    {
        settingMenu.SetActive(false);
      //  GameManager.instance.RestartLevel();
        //Loader.instance.LoadLevel(MyConstants.GAMEPLAY_INDEX);
    }
}
