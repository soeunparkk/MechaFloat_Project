using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BalloonController : MonoBehaviour
{
    public ItemSO balloonData;
    public float currentHP;

    private Coroutine durabilityCoroutine;
    public System.Action OnDurabilityChanged;

    [HideInInspector] public int assignedSlot = -1;
    [HideInInspector] public PlayerPickup owner;

    [Header("UI")]
    public Image hpBarImage; // ü�¹� �̹��� ����

    private void Start()
    {
        if (balloonData != null)
        {
            currentHP = balloonData.maxHP;
            UpdateHPUI();
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

        UpdateHPUI();

        if (currentHP <= 0)
        {
            DestroyBalloon();
        }
    }

    public void StartDurabilityReduction()
    {
        if (durabilityCoroutine == null)
        {
            durabilityCoroutine = StartCoroutine(ReduceDurabilityOverTime());
        }
    }

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
            UpdateHPUI();
            OnDurabilityChanged?.Invoke();
        }

        DestroyBalloon();
    }

    public void SetCurrentDurability(int hp)
    {
        currentHP = Mathf.Clamp(hp, 0, balloonData.maxHP);
        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        if (hpBarImage != null)
        {
            hpBarImage.fillAmount = currentHP / balloonData.maxHP;
        }
    }

    private void DestroyBalloon()
    {
        if (assignedSlot != -1)
        {
            InventoryManager.Instance.RemoveFromInventory(assignedSlot);
        }

        // �ִϸ��̼� ���� �ʱ�ȭ
        if (owner != null && owner.animator != null)
        {
            owner.animator.SetBool("HasBallon", false);
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
                float windStrength = 5f;

                balloonRb.AddForce(windDirection * windStrength, ForceMode.Force);
            }
        }
    }
}
