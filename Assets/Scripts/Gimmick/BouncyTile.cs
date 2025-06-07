using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyTile : MonoBehaviour
{
    [Header("�ٿ ����")]
    [Tooltip("������Ʈ�� ƨ�� �ø��� ���� �����Դϴ�.")]
    public float bounceForce = 20f;
    [Tooltip("���� ������ ����Դϴ�. Impulse�� �������� ū ��, VelocityChange�� ��� �ӵ� �����Դϴ�.")]
    public ForceMode bounceForceMode = ForceMode.Impulse;
    [Tooltip("�� �� ƨ�� �� ���� ������Ʈ�� �ٽ� ƨ�������� �ּ� �ð� (��)�Դϴ�. 0�̸� ��ٿ� ����.")]
    public float bounceCooldown = 0.5f;

    [Header("������Ʈ�� �� ���� (���� ����)")]
    [Tooltip("�÷��̾ ƨ�� �� ����� �� �����Դϴ�.")]
    public float playerBounceMultiplier = 1.0f;
    // �ʿ��ϴٸ� �ٸ� Ư�� �±׳� ������Ʈ�� ���� ������Ʈ�� ���� ������ �߰� ����

    [Header("ȿ�� ����")]
    [Tooltip("ƨ�� �� ����� ����� Ŭ���Դϴ�.")]
    public AudioClip bounceSoundClip;
    [Tooltip("ƨ�� �� ����� ������ �����Դϴ�. (0.0 ~ 1.0)")]
    [Range(0f, 1f)] // Inspector���� �����̴��� ���� ����
    public float bounceSoundVolume = 0.7f;
    [Tooltip("ƨ�� �� ������ ��ƼŬ �ý��� �������Դϴ�. Project â���� �Ҵ��ϼ���.")]
    public GameObject bounceParticlesPrefab; // GameObject Ÿ������ �����Ͽ� ������ ���� �Ҵ�
    [Tooltip("��ƼŬ ���� ��ġ �������Դϴ� (Ÿ�� �߽� ����).")]
    public Vector3 particleOffset = Vector3.up * 0.1f;

    // ���������� ����� ������
    private Dictionary<Rigidbody, float> lastBounceTimes = new Dictionary<Rigidbody, float>();
    private AudioSource audioSource;

    private void Awake() // Start ��� Awake ��� ���� (�ٸ� ��ũ��Ʈ�� Start���� ���� �ʱ�ȭ�� �� �ֵ���)
    {
        // AudioSource ������Ʈ �������ų� �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && bounceSoundClip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        else if (audioSource != null)
        {
            audioSource.playOnAwake = false; // ���� AudioSource�� �ڵ� ��� ����
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;

        if (bounceCooldown > 0 && lastBounceTimes.ContainsKey(rb))
        {
            if (Time.time < lastBounceTimes[rb] + bounceCooldown)
            {
                return;
            }
        }

        float currentBounceForce = bounceForce;
        float currentMultiplier = 1.0f;

        PlayerController player = rb.GetComponent<PlayerController>(); // PlayerController ��ũ��Ʈ�� �ִٰ� ����
        if (player != null)
        {
            currentMultiplier = playerBounceMultiplier;
        }

        Vector3 bounceDirection = transform.up;
        rb.AddForce(bounceDirection * currentBounceForce * currentMultiplier, bounceForceMode);

        if (bounceCooldown > 0)
        {
            lastBounceTimes[rb] = Time.time;
        }
        else if (lastBounceTimes.ContainsKey(rb))
        {
            lastBounceTimes.Remove(rb);
        }

        PlayBounceEffects(collision.contacts[0].point);
    }

    void PlayBounceEffects(Vector3 collisionPoint)
    {
        // ���� ���
        if (bounceSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSoundClip, bounceSoundVolume); // ���� �����Ͽ� ���
        }

        // ��ƼŬ ����
        if (bounceParticlesPrefab != null)
        {
            // ��ƼŬ �����տ��� ParticleSystem ������Ʈ�� ���� �����ͼ� ����ϴ� �ͺ���,
            // GameObject�� Instantiate�ϰ� �ʿ�� ParticleSystem�� ã�� ���� �Ϲ����Դϴ�.
            // ���⼭�� ������ ��ü�� ParticleSystem�� ��Ʈ�� ������ �ִٰ� �����մϴ�.
            GameObject particleInstance = Instantiate(bounceParticlesPrefab, transform.position + particleOffset, bounceParticlesPrefab.transform.rotation);

            // ���� ��ƼŬ�� ������ �����ϰų�, Ư�� �ð� �� �ı��ϰ� �ʹٸ�:
            // ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
            // if (ps != null)
            // {
            //     Destroy(particleInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            // }
            // else // ParticleSystem�� ��Ʈ�� �ƴ� ��� �ڽĿ��� ã�ƾ� �� ���� ����
            // {
            //     Destroy(particleInstance, 5f); // �⺻ �ð� �� �ı� (����)
            // }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        float gizmoLineLength = 1.5f + (bounceForce * 0.05f);
        float arrowHeadLength = 0.35f;
        float arrowHeadAngle = 20.0f;

        Vector3 direction = transform.up;
        Vector3 endPoint = transform.position + direction * gizmoLineLength;

        Gizmos.DrawLine(transform.position, endPoint);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
        Gizmos.DrawLine(endPoint, endPoint + right * arrowHeadLength);
        Gizmos.DrawLine(endPoint, endPoint + left * arrowHeadLength);
    }

    private void OnDisable()
    {
        lastBounceTimes.Clear();
    }
}
