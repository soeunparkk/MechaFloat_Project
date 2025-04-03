using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public Vector3 windDirection = new Vector3(1f, 0f, 0f); 
    public float windStrength = 10f; // 바람의 힘
    public float playerPushStrength = 15f; // 플레이어 밀려나는 힘
    public float balloonPushStrength = 0.5f; // 풍선이 밀려나는 힘
    public float balloonDrag = 0.9f; // 풍선 있을 때 공기 저항
    public float normalDrag = 0.3f; // 기본 공기 저항 값

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            bool hasBalloon = player.BalloonController;
            Debug.Log($" WindZone 적용 - HasBalloon: {hasBalloon}");

            Vector3 force = windDirection.normalized * windStrength;

            if (hasBalloon)
            {
                rb.drag = balloonDrag;
                rb.AddForce(force * 0.1f, ForceMode.Force);
            }
            else
            {
                rb.drag = normalDrag;
                rb.AddForce(force * playerPushStrength, ForceMode.Force);
            }
            return; 
        }

        
        BalloonController balloon = other.GetComponent<BalloonController>();
        if (balloon != null)
        {
            Debug.Log(" 풍선이 바람에 밀림!");
            rb.drag = balloonDrag;
            rb.AddForce(windDirection.normalized * balloonPushStrength, ForceMode.Force);
        }
    }
}
