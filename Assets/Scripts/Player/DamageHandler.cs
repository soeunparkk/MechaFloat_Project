using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    private BalloonController balloonController;

    private void Start()
    {
        balloonController = FindObjectOfType<BalloonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
        {
            Debug.Log($"ǳ���� {other.gameObject.name}�� �浹!");

            // �ӽ�)
            // ��, ���, ��ֹ��� ������ ������ �� �˷��ָ� ���� ����
            balloonController.currentHP -= 5f;

            balloonController.StartDurabilityReduction();
        }
    }
}
