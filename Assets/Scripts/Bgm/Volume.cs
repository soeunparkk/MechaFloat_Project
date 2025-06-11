using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public GameObject settingsPanel;      // 세팅 패널
    public Slider volumeSlider;           // 볼륨 슬라이더

    private bool isSettingsOpen = false;

    void Start()
    {
        // 패널은 처음에 꺼져 있어야 함
        settingsPanel.SetActive(false);

        // 슬라이더 초기값 세팅
        volumeSlider.value = AudioListener.volume;

        // 슬라이더에 값 변경 이벤트 연결
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsPanel();
        }
    }

    void ToggleSettingsPanel()
    {
        isSettingsOpen = !isSettingsOpen;
        settingsPanel.SetActive(isSettingsOpen);
    }

    void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
