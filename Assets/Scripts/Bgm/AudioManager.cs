using System.Collections;
using System.Collections.Generic; // List 사용 등을 위해 (현재는 없지만 일반적)
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // AudioManager에는 AudioSource가 항상 있도록 보장
public class AudioManager : MonoBehaviour
{
    [Header("BGM 클립")]
    public AudioClip stage1BGM; // 변수명 대문자로 시작하는 것이 일반적 (BGM)
    public AudioClip stage2BGM;
    public AudioClip stage3BGM; // <<< 스테이지 3 BGM 추가

    [Header("참조 오브젝트")]
    [Tooltip("플레이어의 Transform 입니다. 플레이어 위치를 기준으로 BGM을 변경합니다.")]
    public Transform playerTransform; // 플레이어 Transform 참조 (이름 명확히)
    [Tooltip("스테이지 2로 넘어가는 기준이 될 타일 또는 지점의 Transform 입니다.")]
    public Transform stage2TriggerTransform; // 스테이지 2 진입 기준점 (이름 명확히)
    [Tooltip("스테이지 3으로 넘어가는 기준이 될 타일 또는 지점의 Transform 입니다.")]
    public Transform stage3TriggerTransform; // <<< 스테이지 3 진입 기준점 추가

    private AudioSource audioSource; // 변수명 audioSource로 변경 (가독성 및 충돌 방지)
    private AudioClip currentPlayingBGM; // 현재 재생 중인 BGM 클립
    private enum CurrentStage { Stage1, Stage2, Stage3 } // 현재 스테이지 상태를 나타내는 열거형
    private CurrentStage currentStageState;

    void Awake() // Start보다 먼저 초기화될 수 있도록 Awake 사용
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioManager: AudioSource 컴포넌트를 찾을 수 없습니다!", gameObject);
            enabled = false; // AudioSource 없으면 비활성화
            return;
        }

        // AudioSource 기본 설정
        audioSource.loop = true; // BGM은 보통 반복 재생
        audioSource.playOnAwake = false; // Start에서 첫 BGM 재생

        // 필수 참조 확인
        if (playerTransform == null)
        {
            Debug.LogError("AudioManager: Player Transform이 할당되지 않았습니다!", gameObject);
            enabled = false; return;
        }
        if (stage1BGM == null) // 스테이지 1 BGM은 필수라고 가정
        {
            Debug.LogError("AudioManager: Stage 1 BGM이 할당되지 않았습니다!", gameObject);
            enabled = false; return;
        }
        if (stage2TriggerTransform == null && stage2BGM != null)
        {
            Debug.LogWarning("AudioManager: Stage 2 BGM은 있으나 Stage 2 Trigger Transform이 할당되지 않았습니다.", gameObject);
        }
        if (stage3TriggerTransform == null && stage3BGM != null)
        {
            Debug.LogWarning("AudioManager: Stage 3 BGM은 있으나 Stage 3 Trigger Transform이 할당되지 않았습니다.", gameObject);
        }
    }

    void Start()
    {
        // 게임 시작 시 스테이지 1 BGM 재생
        currentStageState = CurrentStage.Stage1;
        PlayBGM(stage1BGM);
    }

    void Update() // 매 프레임 플레이어 위치를 확인하여 BGM 변경 여부 결정
    {
        if (playerTransform == null) return; // 플레이어 없으면 실행 안 함

        CheckStageAndChangeBGM();
    }

    void CheckStageAndChangeBGM()
    {
        float playerY = playerTransform.position.y; // 플레이어의 Y 위치 (또는 다른 축 사용 가능)

        // 스테이지 3 조건 확인 (가장 높은 스테이지부터 확인하는 것이 좋음)
        if (stage3TriggerTransform != null && stage3BGM != null && playerY >= stage3TriggerTransform.position.y)
        {
            if (currentStageState != CurrentStage.Stage3)
            {
                currentStageState = CurrentStage.Stage3;
                PlayBGM(stage3BGM);
                Debug.Log("BGM 변경: 스테이지 3");
            }
        }
        // 스테이지 2 조건 확인
        else if (stage2TriggerTransform != null && stage2BGM != null && playerY >= stage2TriggerTransform.position.y)
        {
            if (currentStageState != CurrentStage.Stage2)
            {
                currentStageState = CurrentStage.Stage2;
                PlayBGM(stage2BGM);
                Debug.Log("BGM 변경: 스테이지 2");
            }
        }
        // 기본 스테이지 1 조건 (위 조건들에 해당하지 않으면)
        else
        {
            if (currentStageState != CurrentStage.Stage1 && stage1BGM != null)
            {
                currentStageState = CurrentStage.Stage1;
                PlayBGM(stage1BGM);
                Debug.Log("BGM 변경: 스테이지 1 (기본)");
            }
        }
    }

    void PlayBGM(AudioClip clipToPlay)
    {
        if (clipToPlay == null)
        {
            Debug.LogWarning("재생할 BGM 클립이 null입니다.");
            return;
        }

        // 현재 재생 중인 BGM과 다를 경우에만 변경 및 재생
        if (audioSource.clip != clipToPlay || !audioSource.isPlaying)
        {
            audioSource.Stop(); // 기존 BGM 중지
            audioSource.clip = clipToPlay; // 새 BGM 클립 할당
            audioSource.Play(); // 새 BGM 재생
            currentPlayingBGM = clipToPlay;
        }
    }

    // ChangeAudio 함수는 현재 사용되지 않으므로 주석 처리 또는 다른 용도로 활용 가능
    // void ChangeAudio()
    // {
    //
    // }
}
