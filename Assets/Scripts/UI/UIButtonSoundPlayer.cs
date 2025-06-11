using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 클릭 시 SoundManager를 통해 사운드 재생
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
            Debug.LogWarning("[UIButtonSoundPlayer] SoundManager 인스턴스가 없습니다.");
        }
    }

    private void OnDestroy()
    {
        m_Button.onClick.RemoveListener(PlayClickSound);
    }
}
