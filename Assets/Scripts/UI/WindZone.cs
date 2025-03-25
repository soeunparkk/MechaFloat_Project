using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public Vector3 windDirection = new Vector3(1f, 0f, 0f); // 바람 방향
    public float windStrength = 1f; // 바람의 힘
    public float playerPushStrength = 10f; // 풍선 없을 때 밀려나는 힘
    public float balloonDrag = 2f; // 풍선이 있을 때 공기 저항 효과
    public float normalDrag = 0.5f; // 기본 공기 저항 값

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Vector3 force = windDirection.normalized * windStrength;

            if (player.HasBalloon)
            {
                // 풍선이 있을 경우 천천히 밀리는 효과 + 공기 저항 증가
                rb.drag = balloonDrag;
                rb.AddForce(force * 0.5f, ForceMode.Acceleration);
            }
            else
            {
                // 풍선이 없을 경우 강한 바람으로 빠르게 밀려남
                rb.drag = normalDrag;
                rb.AddForce(force * playerPushStrength, ForceMode.VelocityChange);
            }
        }
    }
}
