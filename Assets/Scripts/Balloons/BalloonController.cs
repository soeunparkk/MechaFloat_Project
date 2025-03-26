using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public ItemSO balloonData;

    private float currentHP;
    private bool isDestroyed = false;

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

        StartCoroutine(ReduceDurabilityOverTime());
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