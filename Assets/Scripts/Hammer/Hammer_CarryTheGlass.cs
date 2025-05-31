using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer_CarryTheGlass : MonoBehaviour
{
    [Header("Movement Settings - 움직임 설정")]
    public float swingAngleMax = 45f;
    public float swingSpeed = 100f;
    public float pauseAtEndpoints = 0.5f;

    [Header("Interaction Settings - 상호작용 설정")]
    public float playerKnockbackForce = 5f;
    public Vector2 knockbackDirectionBias = Vector2.up; // 넉백 시 위로 살짝 띄우는 방향 가중치
    public float knockbackBiasStrength = 0.2f; // 넉백 방향 가중치의 강도 (0~1)

    // 망치가 직접 "Balloon" 태그가 붙은 오브젝트와 충돌했을 때
    public bool destroyBalloonOnDirectHit = true; // 풍선 직접 타격 시 파괴 여부
    public float directBalloonHitForce = 1f;      // 풍선 직접 타격 시 밀어내는 힘 (선택적)

    private bool swingingToEnd = true;
    private float currentTargetAngle;
    private float waitTimer = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        currentTargetAngle = GetAngleFromInitial(-swingAngleMax);
        // 시작 시 초기 각도 즉시 적용 (선택적)
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTargetAngle);
    }

    float GetAngleFromInitial(float offset)
    {
        return initialRotation.eulerAngles.z + offset;
    }

    void Update()
    {
        if (swingingToEnd)
        {
            currentTargetAngle = GetAngleFromInitial(swingAngleMax);
        }
        else
        {
            currentTargetAngle = GetAngleFromInitial(-swingAngleMax);
        }

        float currentZ = transform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, currentTargetAngle, swingSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newZ);

        if (Mathf.Abs(Mathf.DeltaAngle(newZ, currentTargetAngle)) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= pauseAtEndpoints)
            {
                swingingToEnd = !swingingToEnd;
                waitTimer = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 플레이어와 충돌 시
        if (other.CompareTag("Player"))
        {
            HandlePlayerInteraction(other.gameObject);
        }
        // 2. "Balloon" 태그를 가진 오브젝트와 직접 충돌 시 (플레이어가 들고 있든 아니든)
        else if (other.CompareTag("Balloon"))
        {
            HandleDirectBalloonHit(other.gameObject);
        }
    }

    void HandlePlayerInteraction(GameObject playerObject)
    {
        Rigidbody2D playerRb = playerObject.GetComponent<Rigidbody2D>();

        // 플레이어 넉백
        if (playerRb != null)
        {
            Vector2 directionToPlayer = (playerObject.transform.position - transform.position).normalized;
            // 위쪽으로 약간의 바이어스를 주어 띄우는 느낌 추가
            Vector2 finalKnockbackDir = (directionToPlayer + knockbackDirectionBias.normalized * knockbackBiasStrength).normalized;

            playerRb.velocity = Vector2.zero; // 기존 속도 제거
            playerRb.AddForce(finalKnockbackDir * playerKnockbackForce, ForceMode2D.Impulse);
            Debug.Log("Player hit by hammer. Applying knockback.");
        }

        // 플레이어가 풍선을 가지고 있는지 확인 (자식 오브젝트 중 "Balloon" 태그 탐색)
        // 이 방식은 플레이어의 자식으로 풍선이 있고, 해당 풍선에 "Balloon" 태그가 있다고 가정합니다.
        Transform balloonTransform = FindBalloonInChildren(playerObject.transform);

        if (balloonTransform != null)
        {
            Debug.Log("Player was holding a balloon. Deactivating/Destroying balloon.");
            // 풍선 오브젝트를 비활성화하거나 파괴합니다.
            // Destroy(balloonTransform.gameObject); // 즉시 파괴
            balloonTransform.gameObject.SetActive(false); // 비활성화 (나중에 다시 활성화 가능)

            // 여기에 플레이어가 풍선을 잃었을 때의 추가적인 로직을 넣을 수 있습니다.
            // (예: 플레이어 이동 속도 감소, 특정 능력 상실 등 - 플레이어 스크립트에서 처리해야 함)
            // 이 스크립트 단독으로는 플레이어 내부 상태 변경은 어렵습니다.
        }
    }

    void HandleDirectBalloonHit(GameObject balloonObject)
    {
        Debug.Log("Hammer directly hit a Balloon object.");

        if (destroyBalloonOnDirectHit)
        {
            // Destroy(balloonObject); // 즉시 파괴
            balloonObject.SetActive(false); // 비활성화
            Debug.Log("Balloon deactivated/destroyed on direct hit.");
        }
        else
        {
            // 물리적으로 밀어내기 (선택적)
            Rigidbody2D balloonRb = balloonObject.GetComponent<Rigidbody2D>();
            if (balloonRb != null)
            {
                Vector2 directionToBalloon = (balloonObject.transform.position - transform.position).normalized;
                balloonRb.velocity = Vector2.zero;
                balloonRb.AddForce(directionToBalloon * directBalloonHitForce, ForceMode2D.Impulse);
                Debug.Log("Balloon pushed by direct hit.");
            }
        }
    }

    // 자식 오브젝트 중에서 "Balloon" 태그를 가진 첫 번째 오브젝트를 찾는 함수
    Transform FindBalloonInChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Balloon"))
            {
                return child; // 첫 번째로 찾은 풍선 반환
            }
            // 재귀적으로 더 깊은 자식도 탐색하려면 아래 주석 해제
            // Transform found = FindBalloonInChildren(child);
            // if (found != null) return found;
        }
        return null; // 풍선을 찾지 못함
    }
}