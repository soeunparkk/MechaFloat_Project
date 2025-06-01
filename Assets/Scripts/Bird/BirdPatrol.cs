using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPatrol : MonoBehaviour
{
    public float patrolDistance = 5f;  // �Դٰ����� �Ÿ�(�� ����)
    public float speed = 2f;           // �̵� �ӵ�

    private Vector3 startPos;
    private int dir = 1;               // ����: 1(������), -1(����)

    void Start()
    {
        startPos = transform.position; // ���� ��ġ ����
    }

    void Update()
    {
        // �̵�
        transform.Translate(Vector2.right * dir * speed * Time.deltaTime);

        // ��� üũ(��Ʈ�� ���� �ʰ��� ���� ����)
        if (Mathf.Abs(transform.position.x - startPos.x) > patrolDistance * 0.5f)
        {
            dir *= -1; // ���� ����
            // ���� ���(��������Ʈ)�� �¿� �������� �Ѵٸ� �Ʒ� �߰�
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}

