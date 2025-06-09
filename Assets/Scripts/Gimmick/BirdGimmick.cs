
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGimmick : MonoBehaviour, ICheckTrigger // 기존 인터페이스 유지
{
    private PlayerDie playerDie;

    // --- 추가된 설정 변수 ---
    [Header("추가된 설정")]
    [Tooltip("플레이어를 밀어내는 힘의 강도입니다.")]
    public float knockbackForce = 10f;
    [Tooltip("넉백 후 플레이어가 죽기까지의 대기 시간 (초)입니다.")]
    public float delayBeforeDeath = 1.5f;
    // -------------------------

    private bool playerAlreadyHitThisSession = false; // 이 기믹에 의해 이번 세션에 처리되었는지 (중복 방지용)

    private void Start()
    {
        playerDie = FindObjectOfType<PlayerDie>();
        if (playerDie == null)
        {
            Debug.LogError("BirdGimmick: PlayerDie 스크립트를 찾을 수 없습니다!", gameObject);
        }
    }

    public void OnTriggerEntered(Collider other) // 기존 함수 시그니처 유지
    {
        // 기존 조건: 플레이어 태그이고, playerDie 참조가 유효할 때
        if (other.CompareTag("Player") && playerDie != null)
        {
            // --- 추가된 로직 시작 ---
            if (playerAlreadyHitThisSession) // 이미 이 새에 의해 처리되었다면 더 이상 진행 안 함
            {
                return;
            }
            playerAlreadyHitThisSession = true; // 처리됨으로 표시

            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // 넉백 방향: 새의 위치에서 플레이어 쪽으로 (간단하게)
                // 위로 살짝 띄우는 효과 추가
                Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
                knockbackDir = (knockbackDir + Vector3.up * 0.5f).normalized; // Y축 힘 0.5 (조절 가능)

                playerRb.velocity = Vector3.zero; // 기존 속도 초기화 (선택적)
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse); // 넉백!
            }
            else
            {
                Debug.LogWarning("BirdGimmick: 플레이어에 Rigidbody가 없어 넉백을 적용할 수 없습니다.", other.gameObject);
            }

            // 딜레이 후 Die() 함수 호출하는 코루틴 시작
            StartCoroutine(DelayedDeathRoutine());
            // --- 추가된 로직 끝 ---

            // playerDie.Die(); // << 기존의 즉시 사망 코드는 주석 처리하거나 삭제
        }
    }

    // --- 추가된 코루틴 ---
    IEnumerator DelayedDeathRoutine()
    {
        yield return new WaitForSeconds(delayBeforeDeath); // 설정된 시간만큼 대기

        if (playerDie != null) // playerDie가 여전히 유효한지 확인
        {
            playerDie.Die(); // 사망 처리
        }
    }
    // --------------------

    // 만약 이 기믹이 재사용되어야 한다면 (예: 새가 리셋될 때)
    // playerAlreadyHitThisSession 플래그를 false로 되돌리는 함수가 필요할 수 있습니다.
    // public void ResetBirdGimmick()
    // {
    //     playerAlreadyHitThisSession = false;
    // }
}