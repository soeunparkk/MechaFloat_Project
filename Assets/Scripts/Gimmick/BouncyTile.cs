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

    [Header("효과 설정")]
    [Tooltip("튕길 때 재생할 오디오 클립입니다.")]
    public AudioClip bounceSoundClip;
    [Tooltip("튕길 때 재생할 사운드의 볼륨입니다. (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float bounceSoundVolume = 0.7f;

    [Header("파티클 효과 설정")]
    [Tooltip("파티클 효과를 발생시킬 위치를 나타내는 Transform 입니다. (타일의 자식으로 빈 오브젝트를 만들어 할당)")]
    public Transform particleSpawnPoint; // <<< 파티클 생성 위치를 위한 Transform
    [Tooltip("튕길 때 생성할 파티클 시스템 프리팹입니다. Project 창에서 할당하세요.")]
    public GameObject bounceParticlesPrefab;
    [Tooltip("생성될 파티클의 크기 배율입니다. 1은 프리팹의 원래 크기입니다.")]
    public float particleScaleMultiplier = 1.0f;
    // particleSpawnOffset, particleRotationMode, customParticleRotation 등은 이제 필요 없어지거나 역할이 줄어듭니다.
    // particleSpawnPoint의 Transform이 위치와 회전을 모두 결정합니다.

    private Dictionary<Rigidbody, float> lastBounceTimes = new Dictionary<Rigidbody, float>();
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && bounceSoundClip != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        else if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }

        if (bounceParticlesPrefab != null && particleSpawnPoint == null)
        {
            Debug.LogWarning("BouncyTile: bounceParticlesPrefab은 할당되었으나 particleSpawnPoint가 설정되지 않았습니다. 파티클이 타일 위치에서 생성될 수 있습니다.", gameObject);
            // particleSpawnPoint가 없으면 this.transform을 기본값으로 사용할 수 있습니다.
            // particleSpawnPoint = transform; // 또는 경고만 표시
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

        // 충돌 정보는 더 이상 PlayBounceEffects에 필요 없을 수 있음 (particleSpawnPoint 사용 시)
        PlayBounceEffects();
    }

    void PlayBounceEffects() // 파라미터 제거 또는 변경
    {
        // 사운드 재생
        if (bounceSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSoundClip, bounceSoundVolume);
        }

        // 파티클 생성
        if (bounceParticlesPrefab != null)
        {
            Transform spawnTransform = particleSpawnPoint != null ? particleSpawnPoint : transform; // particleSpawnPoint가 없으면 타일 자체 위치 사용

            // particleSpawnPoint의 위치와 회전을 그대로 사용
            GameObject particleInstance = Instantiate(bounceParticlesPrefab, spawnTransform.position, spawnTransform.rotation);

            // 파티클 크기 조절
            if (particleScaleMultiplier != 1.0f)
            {
                particleInstance.transform.localScale *= particleScaleMultiplier;
            }

            // 파티클 자동 파괴 (파티클 프리팹의 Stop Action을 Destroy로 설정하는 것을 권장)
            ParticleSystem ps = particleInstance.GetComponentInChildren<ParticleSystem>(true);
            if (ps != null)
            {
                float lifetime;
                switch (ps.main.startLifetime.mode)
                {
                    case ParticleSystemCurveMode.Constant: lifetime = ps.main.startLifetime.constant; break;
                    case ParticleSystemCurveMode.TwoConstants: lifetime = ps.main.startLifetime.constantMax; break;
                    default:
                        lifetime = ps.main.startLifetime.constantMax * (ps.main.startLifetime.curveMultiplier > 0 ? ps.main.startLifetime.curveMultiplier : 1f);
                        if (lifetime <= 0) lifetime = 5f;
                        break;
                }
                Destroy(particleInstance, Mathf.Max(ps.main.duration, lifetime));
            }
            else
            {
                Destroy(particleInstance, 5f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 바운스 방향 Gizmo
        Gizmos.color = Color.green;
        Vector3 direction = transform.up;
        Vector3 endPoint = transform.position + direction * (1.5f + (bounceForce * 0.05f));
        Gizmos.DrawLine(transform.position, endPoint);
        // ... (화살표 머리 그리는 코드)

        // 파티클 스폰 지점 Gizmo
        if (particleSpawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(particleSpawnPoint.position, 0.2f * particleScaleMultiplier); // 스폰 지점에 구 표시
            Gizmos.DrawLine(transform.position, particleSpawnPoint.position); // 타일 중심에서 스폰 지점까지 선
            // 스폰 지점의 forward 방향 (파티클이 방출될 방향) 표시
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(particleSpawnPoint.position, particleSpawnPoint.forward * 0.5f);
        }
    }

    private void OnDisable()
    {
        lastBounceTimes.Clear();
    }
}