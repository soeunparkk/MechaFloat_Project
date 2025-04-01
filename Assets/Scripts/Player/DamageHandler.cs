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
            Debug.Log($"풍선이 {other.gameObject.name}와 충돌!");

            // 임시)
            // 적, 기믹, 장애물에 닿으면 데미지 값 알려주면 수정 예정
            balloonController.currentHP -= 5f;

            balloonController.StartDurabilityReduction();
        }
    }
}
