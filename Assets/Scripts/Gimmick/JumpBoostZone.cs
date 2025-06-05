using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    public float jumpForce = 20f; // 점프 힘 조절용

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Y축 방향으로 힘을 가함
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // 기존 Y 속도 초기화
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}