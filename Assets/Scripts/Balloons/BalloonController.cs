using System;
using System.Collections;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public ItemSO balloonData;
    public float currentHP;

    private Coroutine durabilityCoroutine;

    public System.Action OnDurabilityChanged;

    [HideInInspector] public int assignedSlot = -1;

    private void Start()
    {
        if (balloonData != null)
        {
            currentHP = balloonData.maxHP;
        }
        else
        {
            Debug.LogError("BalloonController: ǳ�� �����Ͱ� �������� �ʾҽ��ϴ�!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"������ �޾ҽ��ϴ�! ǳ�� ������ ����: {currentHP} (���ҷ�: {damage})");

        if (currentHP <= 0)
        {
            DestroyBalloon();
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
        float durability = balloonData.degradationRate * (balloonData.isReinforced ? balloonData.durabilityMultiplier : 1.0f);

        while (currentHP > 0)
        {
            yield return new WaitForSeconds(2f);
            currentHP -= durability;
            OnDurabilityChanged?.Invoke();
        }

        DestroyBalloon();
    }

    public void SetCurrentDurability(int hp)
    {
        currentHP = Mathf.Clamp(hp, 0, balloonData.maxHP);
    }

    private void DestroyBalloon()
    {
        if (assignedSlot != -1)
        {
            InventoryManager.Instance.RemoveFromInventory(assignedSlot);
        }

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
                float windStrength =5f; // ǳ���� ������ �з����� ���� (�� ���� ����)

                balloonRb.AddForce(windDirection * windStrength, ForceMode.Force);
            }
        }
    }
}
