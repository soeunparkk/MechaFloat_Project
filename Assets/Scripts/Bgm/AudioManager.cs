using System.Collections;
using System.Collections.Generic; // List ��� ���� ���� (����� ������ �Ϲ���)
using UnityEngine;

[RequireComponent(typeof(AudioSource))] // AudioManager���� AudioSource�� �׻� �ֵ��� ����
public class AudioManager : MonoBehaviour
{
    [Header("BGM Ŭ��")]
    public AudioClip stage1BGM; // ������ �빮�ڷ� �����ϴ� ���� �Ϲ��� (BGM)
    public AudioClip stage2BGM;
    public AudioClip stage3BGM; // <<< �������� 3 BGM �߰�

    [Header("���� ������Ʈ")]
    [Tooltip("�÷��̾��� Transform �Դϴ�. �÷��̾� ��ġ�� �������� BGM�� �����մϴ�.")]
    public Transform playerTransform; // �÷��̾� Transform ���� (�̸� ��Ȯ��)
    [Tooltip("�������� 2�� �Ѿ�� ������ �� Ÿ�� �Ǵ� ������ Transform �Դϴ�.")]
    public Transform stage2TriggerTransform; // �������� 2 ���� ������ (�̸� ��Ȯ��)
    [Tooltip("�������� 3���� �Ѿ�� ������ �� Ÿ�� �Ǵ� ������ Transform �Դϴ�.")]
    public Transform stage3TriggerTransform; // <<< �������� 3 ���� ������ �߰�

    private AudioSource audioSource; // ������ audioSource�� ���� (������ �� �浹 ����)
    private AudioClip currentPlayingBGM; // ���� ��� ���� BGM Ŭ��
    private enum CurrentStage { Stage1, Stage2, Stage3 } // ���� �������� ���¸� ��Ÿ���� ������
    private CurrentStage currentStageState;

    void Awake() // Start���� ���� �ʱ�ȭ�� �� �ֵ��� Awake ���
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioManager: AudioSource ������Ʈ�� ã�� �� �����ϴ�!", gameObject);
            enabled = false; // AudioSource ������ ��Ȱ��ȭ
            return;
        }

        // AudioSource �⺻ ����
        audioSource.loop = true; // BGM�� ���� �ݺ� ���
        audioSource.playOnAwake = false; // Start���� ù BGM ���

        // �ʼ� ���� Ȯ��
        if (playerTransform == null)
        {
            Debug.LogError("AudioManager: Player Transform�� �Ҵ���� �ʾҽ��ϴ�!", gameObject);
            enabled = false; return;
        }
        if (stage1BGM == null) // �������� 1 BGM�� �ʼ���� ����
        {
            Debug.LogError("AudioManager: Stage 1 BGM�� �Ҵ���� �ʾҽ��ϴ�!", gameObject);
            enabled = false; return;
        }
        if (stage2TriggerTransform == null && stage2BGM != null)
        {
            Debug.LogWarning("AudioManager: Stage 2 BGM�� ������ Stage 2 Trigger Transform�� �Ҵ���� �ʾҽ��ϴ�.", gameObject);
        }
        if (stage3TriggerTransform == null && stage3BGM != null)
        {
            Debug.LogWarning("AudioManager: Stage 3 BGM�� ������ Stage 3 Trigger Transform�� �Ҵ���� �ʾҽ��ϴ�.", gameObject);
        }
    }

    void Start()
    {
        // ���� ���� �� �������� 1 BGM ���
        currentStageState = CurrentStage.Stage1;
        PlayBGM(stage1BGM);
    }

    void Update() // �� ������ �÷��̾� ��ġ�� Ȯ���Ͽ� BGM ���� ���� ����
    {
        if (playerTransform == null) return; // �÷��̾� ������ ���� �� ��

        CheckStageAndChangeBGM();
    }

    void CheckStageAndChangeBGM()
    {
        float playerY = playerTransform.position.y; // �÷��̾��� Y ��ġ (�Ǵ� �ٸ� �� ��� ����)

        // �������� 3 ���� Ȯ�� (���� ���� ������������ Ȯ���ϴ� ���� ����)
        if (stage3TriggerTransform != null && stage3BGM != null && playerY >= stage3TriggerTransform.position.y)
        {
            if (currentStageState != CurrentStage.Stage3)
            {
                currentStageState = CurrentStage.Stage3;
                PlayBGM(stage3BGM);
                Debug.Log("BGM ����: �������� 3");
            }
        }
        // �������� 2 ���� Ȯ��
        else if (stage2TriggerTransform != null && stage2BGM != null && playerY >= stage2TriggerTransform.position.y)
        {
            if (currentStageState != CurrentStage.Stage2)
            {
                currentStageState = CurrentStage.Stage2;
                PlayBGM(stage2BGM);
                Debug.Log("BGM ����: �������� 2");
            }
        }
        // �⺻ �������� 1 ���� (�� ���ǵ鿡 �ش����� ������)
        else
        {
            if (currentStageState != CurrentStage.Stage1 && stage1BGM != null)
            {
                currentStageState = CurrentStage.Stage1;
                PlayBGM(stage1BGM);
                Debug.Log("BGM ����: �������� 1 (�⺻)");
            }
        }
    }

    void PlayBGM(AudioClip clipToPlay)
    {
        if (clipToPlay == null)
        {
            Debug.LogWarning("����� BGM Ŭ���� null�Դϴ�.");
            return;
        }

        // ���� ��� ���� BGM�� �ٸ� ��쿡�� ���� �� ���
        if (audioSource.clip != clipToPlay || !audioSource.isPlaying)
        {
            audioSource.Stop(); // ���� BGM ����
            audioSource.clip = clipToPlay; // �� BGM Ŭ�� �Ҵ�
            audioSource.Play(); // �� BGM ���
            currentPlayingBGM = clipToPlay;
        }
    }

    // ChangeAudio �Լ��� ���� ������ �����Ƿ� �ּ� ó�� �Ǵ� �ٸ� �뵵�� Ȱ�� ����
    // void ChangeAudio()
    // {
    //
    // }
}
