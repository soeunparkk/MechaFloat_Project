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
            Debug.LogError("BalloonController: ǳ�� �����Ͱ� �������� �ʾҽ��ϴ�!");
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

            Debug.Log($"ǳ�� ������ ����: {currentHP} (���ҷ�: {damage})");

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
            Debug.Log($"ǳ���� {other.gameObject.name}�� �浹!");
            DestroyBalloon();
        }
    }

    private void DestroyBalloon()
    {
        isDestroyed = true;
        Debug.Log("ǳ���� �������ϴ�!");
        Destroy(gameObject);
    }
}