using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
     public float windStrength = 10f; 
    public float playerPushStrength = 15f;  플레이어 밀려나는 힘
    public float balloonPushStrength = 5f; /
    public float balloonDrag = 0.8f; 
    public float normalDrag = 0.3f; 

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 force = transform.forward.normalized * windStrength; 

        
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.HasBalloon)
            {
                rb.drag = balloonDrag;
                rb.AddForce(force * 0.5f, ForceMode.Force); // 풍선 있을 때 천천히
            }
            else
            {
                rb.drag = normalDrag;
                rb.AddForce(force * playerPushStrength, ForceMode.Force); // 풍선 없을 때 강하게
            }
            return;
        }

        // 풍선에도 바람 적용
        BalloonController balloon = other.GetComponent<BalloonController>();
        if (balloon != null)
        {
            rb.drag = balloonDrag;
            rb.AddForce(force * balloonPushStrength, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.drag = normalDrag;
        }
    }

}
