using System;
using UnityEngine;

public enum SFXType
{
    ButtonTap, Claim, RvActivateSound, RingDestroySound, coinCollect
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioSource bgMusic;
    [SerializeField] AudioClip[] clips;
    [SerializeField] private AudioSource sfxClip;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scene
    }

    private void Start()
    {
        Vibration.Init();
    }

    public void PlaySFX(SFXType type)
    {
        switch (type)
        {
            case SFXType.ButtonTap:
                sfxClip.PlayOneShot(clips[0]);
                break;
            case SFXType.Claim:
                sfxClip.PlayOneShot(clips[1]);
                break;
            case SFXType.RvActivateSound:
                sfxClip.PlayOneShot(clips[2]);
                break;
            case SFXType.RingDestroySound:
                sfxClip.PlayOneShot(clips[3]);
                break;
        }
    }
    
    public void ToogleSound(bool state)
    {
        sfxClip.mute = !state;
    }

    public void ToogleMusic(bool state)
    {
        bgMusic.mute = !state;
    }
}
