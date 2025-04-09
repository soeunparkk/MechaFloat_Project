using System;
using System.Collections;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public ItemSO balloonData;
    public float currentHP;

    private Coroutine durabilityCoroutine;

    private void Start()
    {
        if (balloonData != null)
        {
            currentHP = balloonData.maxHP;
        }
        else
        {
            Debug.LogError("BalloonController: 풍선 데이터가 설정되지 않았습니다!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"공격을 받았습니다! 풍선 내구도 감소: {currentHP} (감소량: {damage})");

        if (currentHP <= 0)
        {
            DestroyBalloon();
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
            durabilityCoroutine = null;
        }
    }

    private IEnumerator ReduceDurabilityOverTime()
    {
        float durability = balloonData.degradationRate * (balloonData.isReinforced ? balloonData.durabilityMultiplier : 1.0f);

        while (currentHP > 0)
        {
            yield return new WaitForSeconds(2f);
            currentHP -= durability;

            Debug.Log($"풍선 내구도 감소: {currentHP} (감소량: {durability})");
        }
    }

    public void SetCurrentDurability(int hp)
    {
        currentHP = Mathf.Clamp(hp, 0, balloonData.maxHP);

        // 필요한 경우 내구도 UI도 갱신
    }

    private void DestroyBalloon()
    {
        Debug.Log("풍선이 터졌습니다!");
        // 풍선 터지는 애니메이션 받으면 추가 예정 일단은 0이되면 파괴만
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WindZone"))
        {
            Rigidbody balloonRb = GetComponent<Rigidbody>();
            if (balloonRb != null)
            {
                Vector3 windDirection = other.transform.forward; 
                float windStrength =5f; // 풍선이 가볍게 밀려나는 정도 (값 조절 가능)

                balloonRb.AddForce(windDirection * windStrength, ForceMode.Force);
            }
        }
    }
}
