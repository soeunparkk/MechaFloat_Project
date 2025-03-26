using System.Collections;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public ItemSO balloonData;

    private float currentHP;
    private bool isDestroyed = false;
    private Coroutine durabilityCoroutine;

    private void Start()
    {
        if (balloonData != null)
        {
            currentHP = balloonData.HP;
        }
        else
        {
            Debug.LogError("BalloonController: 풍선 데이터가 설정되지 않았습니다!");
        }
    }

    // 픽업 시 내구도 감소 시작
    public void StartDurabilityReduction()
    {
        if (durabilityCoroutine == null) // 중복 실행 방지
        {
            durabilityCoroutine = StartCoroutine(ReduceDurabilityOverTime());
        }
    }

    // 해제 시 내구도 감소 멈춤
    public void StopDurabilityReduction()
    {
        if (durabilityCoroutine != null)
        {
            StopCoroutine(durabilityCoroutine);
            durabilityCoroutine = null; // 초기화
        }
    }

    private IEnumerator ReduceDurabilityOverTime()
    {
        float damage = balloonData.isReinforced ? 0.5f : 1.0f;

        while (currentHP > 0)
        {
            yield return new WaitForSeconds(1f);
            currentHP -= damage;

            Debug.Log($"풍선 내구도 감소: {currentHP} (감소량: {damage})");

            if (currentHP <= 0)
            {
                DestroyBalloon();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log($"풍선이 {other.gameObject.name}와 충돌!");
            DestroyBalloon();
        }
    }

    private void DestroyBalloon()
    {
        isDestroyed = true;
        Debug.Log("풍선이 터졌습니다!");
        Destroy(gameObject);
    }
}
