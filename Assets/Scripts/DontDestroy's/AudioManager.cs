using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioInstance;
    public AudioMixer MasterMixer;
    public AudioSource SFXSource;
    public AudioSource BGMSource;
    public AudioClip GunshotSFX;
    public AudioClip DeathSFX;
    private void Awake()
    {
        if (AudioInstance == null)
        {
            AudioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ApplySavedVolumes();
    }
    public void SetMasterVolume(float volume)
    {
        MasterMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetBGMVolume(float volume)
    {
        MasterMixer.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        MasterMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat("MasterVolume", 0);
    }
    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat("BGMVolume", 0);
    }
    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 0);
    }
    
    public void PlaySFX( AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    private void ApplySavedVolumes()
    {
        MasterMixer.SetFloat("MasterVolume", GetMasterVolume());
        MasterMixer.SetFloat("BGMVolume", GetBGMVolume());
        MasterMixer.SetFloat("SFXVolume", GetSFXVolume());
    }
}
