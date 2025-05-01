using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    [Header("바람 설정")]
    public Vector3 windDirection = Vector3.forward; // 에디터에서 직접 설정 가능
    public float windStrength = 10f;

    [Header("힘 조절")]
    public float playerPushStrength = 15f;
    public float balloonPushStrength = 5f;

    [Header("Drag 설정")]
    public float balloonDrag = 0.8f;
    public float normalDrag = 0.3f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 force = windDirection.normalized * windStrength;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.HasBalloon)
            {
                rb.drag = balloonDrag;
                rb.AddForce(force * 0.5f, ForceMode.Force);
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