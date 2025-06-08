// ���� �̸�: SimpleUISound.cs
using UnityEngine;
using UnityEngine.EventSystems; // IPointerClickHandler ����� ����

public class SimpleUISound : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Ŭ�� �� ����� ����� Ŭ���Դϴ�.")]
    public AudioClip clickSound;

    [Tooltip("���带 ����� AudioSource�Դϴ�. ����θ� �� ���� ������Ʈ���� ã�ų� ���� �߰��մϴ�.")]
    public AudioSource audioSource; // ���� �Ҵ��ϰų�, ������ Awake���� ó��

    [Tooltip("Ŭ�� ������ �����Դϴ�.")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    void Awake()
    {
        // Inspector���� AudioSource�� �Ҵ���� �ʾҴٸ�, �� ���� ������Ʈ���� ã�ƺ��ϴ�.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // �׷��� AudioSource�� ����, ����� ���� Ŭ���� �ִٸ� ���� �߰��մϴ�.
        if (audioSource == null && clickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // AudioSource �⺻ ���� (Inspector������ ����)
        if (audioSource != null)
        {
            audioSource.playOnAwake = false; // �� ���� �� �ڵ� ��� ����
            audioSource.loop = false;        // Ŭ������ ���� �ݺ����� ����
        }
    }

    // IPointerClickHandler �������̽��� �Լ� ����
    // �� ��ũ��Ʈ�� ���� UI ��Ұ� Ŭ���Ǹ� �� �Լ��� ȣ��˴ϴ�.
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
    }

    // �ܺο����� ȣ�� �����ϵ��� public���� ���� ���� �ֽ��ϴ�.
    // (��: UI Button�� OnClick() �̺�Ʈ�� ���� �����Ͽ� ���)
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            // ������ �����Ͽ� ���� ���
            audioSource.PlayOneShot(clickSound, volume);
        }
        else
        {
            if (clickSound == null)
            {
                Debug.LogWarning("SimpleUISound: 'Click Sound' ����� Ŭ���� �Ҵ���� �ʾҽ��ϴ�.", gameObject);
            }
            if (audioSource == null)
            {
                // clickSound�� null�� �� AudioSource�� �������� �ʵ��� Awake�� ���������Ƿ�,
                // �� ���� clickSound�� ������ AudioSource ������ ������ ���� �幮 ��츦 ���� ��.
                // �Ǵ� Awake ���� AudioSource�� � ������ null�� �� ���.
                Debug.LogWarning("SimpleUISound: AudioSource ������Ʈ�� ã�ų� ������ �� �����ϴ�. 'Click Sound'�� �Ҵ�Ǿ� �ִ��� Ȯ���ϼ���.", gameObject);
            }
        }
    }
}