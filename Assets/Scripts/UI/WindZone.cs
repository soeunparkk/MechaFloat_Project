using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public Vector3 windDirection = new Vector3(1f, 0f, 0f); // �ٶ� ����
    public float windStrength = 1f; // �ٶ��� ��
    public float playerPushStrength = 10f; // ǳ�� ���� �� �з����� ��
    public float balloonDrag = 2f; // ǳ���� ���� �� ���� ���� ȿ��
    public float normalDrag = 0.5f; // �⺻ ���� ���� ��

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
                // ǳ���� ���� ��� õõ�� �и��� ȿ�� + ���� ���� ����
                rb.drag = balloonDrag;
                rb.AddForce(force * 0.5f, ForceMode.Acceleration);
            }
            else
            {
                // ǳ���� ���� ��� ���� �ٶ����� ������ �з���
                rb.drag = normalDrag;
                rb.AddForce(force * playerPushStrength, ForceMode.VelocityChange);
            }
        }
    }
}
