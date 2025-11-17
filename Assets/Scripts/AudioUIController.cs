using UnityEngine;
using UnityEngine.UI;

public class AudioUIController : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        float master = AudioManager.AudioInstance.GetMasterVolume();
        float bgm = AudioManager.AudioInstance.GetBGMVolume();
        float sfx = AudioManager.AudioInstance.GetSFXVolume();
        
        masterSlider.value = master;
        bgmSlider.value = bgm;
        sfxSlider.value = sfx;

        AudioManager.AudioInstance.SetMasterVolume(master);
        AudioManager.AudioInstance.SetBGMVolume(bgm);
        AudioManager.AudioInstance.SetSFXVolume(sfx);

        masterSlider.onValueChanged.AddListener(AudioManager.AudioInstance.SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(AudioManager.AudioInstance.SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.AudioInstance.SetSFXVolume);

    }
}
