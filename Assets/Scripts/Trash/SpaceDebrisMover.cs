using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ǳ���� �� �� �ִ� ������ Damage ó�� (���ϴ´�� Ŀ���� ����!)
public class Balloon : MonoBehaviour
{
    public int hp = 3;

    public void Damage(int amount)
    {
        hp -= amount;
        Debug.Log("ǳ�� ������! ���� HP: " + hp);
        if (hp <= 0)
        {
            Destroy(gameObject); // ����
        }
    }
}

// ���־����� �̵� + �浹 üũ �ѹ���!
public class SpaceDebris : MonoBehaviour
{
    public float moveSpeed = 1.0f; // �̵� �ӵ�
    public Vector3 moveDirection;   // �̵� ����

    void Start()
    {
        // Rigidbody�� ������ ���߷����� (use gravity ����)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = false;

        // ���� �ڵ� ���� ����
        if (moveDirection == Vector3.zero)
            moveDirection = Random.onUnitSphere;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        // Player(Ȥ�� ǳ��)�� �±׸� "Player"�� �����ߴٰ� ����
        if (other.CompareTag("Player"))
        {
            // ǳ�� ��ũ��Ʈ ������Ʈ ã�Ƽ� ������ ó��
            Balloon balloon = other.GetComponent<Balloon>();
            if (balloon != null)
            {
                balloon.Damage(1);
            }
            // �� ������ ������Ʈ ����(�ɼ�)
            // Destroy(gameObject);
        }
    }
}