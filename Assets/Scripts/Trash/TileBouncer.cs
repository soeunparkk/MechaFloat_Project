using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBouncer : MonoBehaviour
{
    public float oscillateSpeed = 2f;    // �����̴� �ӵ�
    public float oscillateHeight = 0.5f; // �̵� ����
    public float minY = 0f;              // y�� ���� �Ѱ谪(�ٴ� ��ġ)

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * oscillateSpeed) * oscillateHeight;
        float targetY = initialPosition.y + offsetY;

        // �ٴ� ���Ϸ� �� �������� ����
        if (targetY < minY)
            targetY = minY;

        transform.position = new Vector3(initialPosition.x, targetY, initialPosition.z);
    }
}