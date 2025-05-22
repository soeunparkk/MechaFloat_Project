using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTile : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // ȸ���� (�⺻�� Y��)
    public float rotationSpeed = 45f; // �ʴ� ȸ�� �ӵ� (�� ����)

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
