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
            durabilityCoroutine = null; // �ʱ�ȭ
        }
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
