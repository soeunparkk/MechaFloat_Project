using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider musicMasterSlider;
    [SerializeField] private Slider musicBGMSlider;
    [SerializeField] private Slider musicSFXSlider;

    private void Awake()
    {
        musicMasterSlider.value = PlayerPrefs.GetFloat("Volume_Master", 1f);
        musicBGMSlider.value = PlayerPrefs.GetFloat("Volume_BGM", 1f);
        musicSFXSlider.value = PlayerPrefs.GetFloat("Volume_SFX", 1f);

        musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        musicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)                           // 마스터 볼륨 슬라이더가 Mixer에 반영되게
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);        // 볼륨은 Log10 단위에 x20을 해준다.
        PlayerPrefs.SetFloat("Volume_Master", volume);
    }

    public void SetBGMVolume(float volume)                              // BGM 볼륨 슬라이더가 Mixer에 반영되게
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume_BGM", volume);
    }

    public void SetSFXVolume(float volume)                              // SFX 볼륨 슬라이더가 Mixer에 반영되게
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume_SFX", volume);
    }
}