using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ư Ŭ�� �� SoundManager�� ���� ���� ���
/// </summary>
[RequireComponent(typeof(Button))]
public class UIButtonSoundPlayer : MonoBehaviour
{
    [SerializeField] private string soundName = "Click";

    private Button m_Button;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(soundName);
        }
        else
        {
            Debug.LogWarning("[UIButtonSoundPlayer] SoundManager �ν��Ͻ��� �����ϴ�.");
        }
    }

    private void OnDestroy()
    {
        m_Button.onClick.RemoveListener(PlayClickSound);
    }
}
