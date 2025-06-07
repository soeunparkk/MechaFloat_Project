using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyTile : MonoBehaviour
{
    [Header("바운스 설정")]
    [Tooltip("오브젝트를 튕겨 올리는 힘의 강도입니다.")]
    public float bounceForce = 20f;
    [Tooltip("힘을 적용할 방식입니다. Impulse는 순간적인 큰 힘, VelocityChange는 즉시 속도 변경입니다.")]
    public ForceMode bounceForceMode = ForceMode.Impulse;
    [Tooltip("한 번 튕긴 후 동일 오브젝트가 다시 튕기기까지의 최소 시간 (초)입니다. 0이면 쿨다운 없음.")]
    public float bounceCooldown = 0.5f;

    [Header("오브젝트별 힘 배율 (선택 사항)")]
    [Tooltip("플레이어가 튕길 때 적용될 힘 배율입니다.")]
    public float playerBounceMultiplier = 1.0f;
    // 필요하다면 다른 특정 태그나 컴포넌트를 가진 오브젝트에 대한 배율도 추가 가능

    [Header("효과 설정")]
    [Tooltip("튕길 때 재생할 오디오 클립입니다.")]
    public AudioClip bounceSoundClip;
    [Tooltip("튕길 때 재생할 사운드의 볼륨입니다. (0.0 ~ 1.0)")]
    [Range(0f, 1f)] // Inspector에서 슬라이더로 조절 가능
    public float bounceSoundVolume = 0.7f;
    [Tooltip("튕길 때 생성할 파티클 시스템 프리팹입니다. Project 창에서 할당하세요.")]
    public GameObject bounceParticlesPrefab; // GameObject 타입으로 변경하여 프리팹 직접 할당
    [Tooltip("파티클 생성 위치 오프셋입니다 (타일 중심 기준).")]
    public Vector3 particleOffset = Vector3.up * 0.1f;

    // 내부적으로 사용할 변수들
    private Dictionary<Rigidbody, float> lastBounceTimes = new Dictionary<Rigidbody, float>();
    private AudioSource audioSource;

    private void Awake() // Start 대신 Awake 사용 권장 (다른 스크립트의 Start보다 먼저 초기화될 수 있도록)
    {
        // AudioSource 컴포넌트 가져오거나 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && bounceSoundClip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        else if (audioSource != null)
        {
            audioSource.playOnAwake = false; // 기존 AudioSource도 자동 재생 방지
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

        PlayerController player = rb.GetComponent<PlayerController>(); // PlayerController 스크립트가 있다고 가정
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
        // 사운드 재생
        if (bounceSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSoundClip, bounceSoundVolume); // 볼륨 지정하여 재생
        }

        // 파티클 생성
        if (bounceParticlesPrefab != null)
        {
            // 파티클 프리팹에서 ParticleSystem 컴포넌트를 직접 가져와서 사용하는 것보다,
            // GameObject로 Instantiate하고 필요시 ParticleSystem을 찾는 것이 일반적입니다.
            // 여기서는 프리팹 자체가 ParticleSystem을 루트에 가지고 있다고 가정합니다.
            GameObject particleInstance = Instantiate(bounceParticlesPrefab, transform.position + particleOffset, bounceParticlesPrefab.transform.rotation);

            // 만약 파티클의 수명을 관리하거나, 특정 시간 후 파괴하고 싶다면:
            // ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
            // if (ps != null)
            // {
            //     Destroy(particleInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            // }
            // else // ParticleSystem이 루트가 아닌 경우 자식에서 찾아야 할 수도 있음
            // {
            //     Destroy(particleInstance, 5f); // 기본 시간 후 파괴 (예시)
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
