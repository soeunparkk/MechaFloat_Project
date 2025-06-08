// 파일 이름: SimpleUISound.cs
using UnityEngine;
using UnityEngine.EventSystems; // IPointerClickHandler 사용을 위해

public class SimpleUISound : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("클릭 시 재생할 오디오 클립입니다.")]
    public AudioClip clickSound;

    [Tooltip("사운드를 재생할 AudioSource입니다. 비워두면 이 게임 오브젝트에서 찾거나 새로 추가합니다.")]
    public AudioSource audioSource; // 직접 할당하거나, 없으면 Awake에서 처리

    [Tooltip("클릭 사운드의 볼륨입니다.")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    void Awake()
    {
        // Inspector에서 AudioSource가 할당되지 않았다면, 이 게임 오브젝트에서 찾아봅니다.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // 그래도 AudioSource가 없고, 재생할 사운드 클립이 있다면 새로 추가합니다.
        if (audioSource == null && clickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // AudioSource 기본 설정 (Inspector에서도 가능)
        if (audioSource != null)
        {
            audioSource.playOnAwake = false; // 씬 시작 시 자동 재생 방지
            audioSource.loop = false;        // 클릭음은 보통 반복하지 않음
        }
    }

    // IPointerClickHandler 인터페이스의 함수 구현
    // 이 스크립트가 붙은 UI 요소가 클릭되면 이 함수가 호출됩니다.
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
    }

    // 외부에서도 호출 가능하도록 public으로 만들 수도 있습니다.
    // (예: UI Button의 OnClick() 이벤트에 직접 연결하여 사용)
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            // 볼륨을 적용하여 사운드 재생
            audioSource.PlayOneShot(clickSound, volume);
        }
        else
        {
            if (clickSound == null)
            {
                Debug.LogWarning("SimpleUISound: 'Click Sound' 오디오 클립이 할당되지 않았습니다.", gameObject);
            }
            if (audioSource == null)
            {
                // clickSound가 null일 때 AudioSource를 생성하지 않도록 Awake를 수정했으므로,
                // 이 경고는 clickSound는 있지만 AudioSource 생성에 실패한 극히 드문 경우를 위한 것.
                // 또는 Awake 이후 AudioSource가 어떤 이유로 null이 된 경우.
                Debug.LogWarning("SimpleUISound: AudioSource 컴포넌트를 찾거나 생성할 수 없습니다. 'Click Sound'가 할당되어 있는지 확인하세요.", gameObject);
            }
        }
    }
}