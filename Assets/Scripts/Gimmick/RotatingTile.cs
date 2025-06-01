using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotatingTile : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 45f;

    private Rigidbody rb;
    private Quaternion previousRotation;

    private HashSet<Transform> rotatingObjects = new HashSet<Transform>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        previousRotation = rb.rotation;
    }

    void FixedUpdate()
    {
        // ȸ�� ���
        Quaternion deltaRotation = Quaternion.Euler(rotationAxis * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // �� �÷��̾ ȸ�� ��Ÿ��ŭ ȸ����Ŵ
        foreach (Transform obj in rotatingObjects)
        {
            obj.RotateAround(transform.position, rotationAxis, rotationSpeed * Time.fixedDeltaTime);
        }

        previousRotation = rb.rotation;

        foreach (Transform obj in rotatingObjects)
        {
            // 1. ��ġ ȸ��
            obj.RotateAround(transform.position, rotationAxis, rotationSpeed * Time.fixedDeltaTime);

            // 2. ���� ȸ�� (���� ���� ����)
            Quaternion rotationDelta = Quaternion.Euler(rotationAxis * rotationSpeed * Time.fixedDeltaTime);
            obj.rotation = rotationDelta * obj.rotation;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rotatingObjects.Add(collision.transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rotatingObjects.Remove(collision.transform);
        }
    }
}