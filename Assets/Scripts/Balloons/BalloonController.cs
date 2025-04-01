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
            currentHP = balloonData.HP;
        }
        else
        {
            Debug.LogError("BalloonController: ǳ�� �����Ͱ� �������� �ʾҽ��ϴ�!");
        }
    }

    // �Ⱦ� �� ������ ���� ����
    public void StartDurabilityReduction()
    {
        if (durabilityCoroutine == null) // �ߺ� ���� ����
        {
            durabilityCoroutine = StartCoroutine(ReduceDurabilityOverTime());
        }
    }

    // ���� �� ������ ���� ����
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
        float damage = balloonData.isReinforced ? 0.5f : 1.0f;

        while (currentHP > 0)
        {
            yield return new WaitForSeconds(2f);
            currentHP -= damage;

            Debug.Log($"ǳ�� ������ ����: {currentHP} (���ҷ�: {damage})");

            if (currentHP <= 0)
            {
                DestroyBalloon();
            }
        }
    }

    private void DestroyBalloon()
    {
        Debug.Log("ǳ���� �������ϴ�!");
        // ǳ�� ������ �ִϸ��̼� ������ �߰� ���� �ϴ��� 0�̵Ǹ� �ı���
        Destroy(gameObject);
    }
}
