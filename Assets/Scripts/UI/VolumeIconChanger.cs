using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeIconChanger : MonoBehaviour
{
    public Slider volumeSlider; 
    public Image volumeImage;   
    public Sprite volumeMute;
    public Sprite volumeOff;
    public Sprite volumeDown;
    public Sprite volumeUp;

    public AudioSource audioSource; 

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        UpdateVolume(volumeSlider.value);
    }

    private void UpdateVolume(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
        }

        if (value == 0)
        {
            volumeImage.sprite = volumeMute;
        }
        else if (value > 0 && value <= 0.3f)
        {
            volumeImage.sprite = volumeOff;
        }
        else if (value > 0.3f && value <= 0.7f)
        {
            volumeImage.sprite = volumeDown;
        }
        else
        {
            volumeImage.sprite = volumeUp;
        }
    }
}
