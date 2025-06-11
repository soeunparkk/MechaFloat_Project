using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public GameObject settingsPanel;      // ���� �г�
    public Slider volumeSlider;           // ���� �����̴�

    private bool isSettingsOpen = false;

    void Start()
    {
        // �г��� ó���� ���� �־�� ��
        settingsPanel.SetActive(false);

        // �����̴� �ʱⰪ ����
        volumeSlider.value = AudioListener.volume;

        // �����̴��� �� ���� �̺�Ʈ ����
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
