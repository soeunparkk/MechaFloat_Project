using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float playerPushStrength = 15f;  // 풍선 없을 때 밀려나는 힘
    public float balloonPushStrength = 5f;  // 풍선 있을 때 밀려나는 힘
    public float balloonDrag = 1.2f;       // 풍선 drag 값
    public float normalDrag = 0.3f;        // 기본 drag 값
    public float normalMass = 1f;          // 기본 mass 값
    public float balloonMass = 0.5f;       // 풍선 있을 때 mass 값 (작게 설정)

    public Vector3 windDirection = new Vector3(1f, 0f, 0f);  // 바람 방향

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            bool hasBalloon = player.HasBalloon;

            // 바람 힘 계산
            Vector3 force = windDirection.normalized;

            if (hasBalloon)
            {
                // 풍선 있을 때 → 밀리는 힘 약하게, mass 작게, drag 크게
                rb.mass = balloonMass;
                rb.drag = balloonDrag;
                rb.AddForce(force * balloonPushStrength, ForceMode.Force);
                Debug.Log("풍선 있음 - 덜 밀림");
            }
            else
            {
                // 풍선 없을 때 → 밀리는 힘 강하게, mass 크게, drag 작게
                rb.mass = normalMass;
                rb.drag = normalDrag;
                rb.AddForce(force * playerPushStrength, ForceMode.Force);
                Debug.Log("풍선 없음 - 세게 밀림");
            }
            return;
        }

        // 풍선이 바람에 밀리는 경우
        BalloonController balloon = other.GetComponent<BalloonController>();
        if (balloon != null)
        {
            rb.drag = balloonDrag;
            rb.AddForce(windDirection.normalized * balloonPushStrength, ForceMode.Force);
        }
    }
}
