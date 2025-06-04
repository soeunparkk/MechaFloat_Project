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

    public void SetMasterVolume(float volume)                           // ������ ���� �����̴��� Mixer�� �ݿ��ǰ�
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);        // ������ Log10 ������ x20�� ���ش�.
        PlayerPrefs.SetFloat("Volume_Master", volume);
    }

    public void SetBGMVolume(float volume)                              // BGM ���� �����̴��� Mixer�� �ݿ��ǰ�
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume_BGM", volume);
    }

    public void SetSFXVolume(float volume)                              // SFX ���� �����̴��� Mixer�� �ݿ��ǰ�
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume_SFX", volume);
    }
}