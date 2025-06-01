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
        // 회전 계산
        Quaternion deltaRotation = Quaternion.Euler(rotationAxis * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // ▶ 플레이어도 회전 델타만큼 회전시킴
        foreach (Transform obj in rotatingObjects)
        {
            obj.RotateAround(transform.position, rotationAxis, rotationSpeed * Time.fixedDeltaTime);
        }

        previousRotation = rb.rotation;

        foreach (Transform obj in rotatingObjects)
        {
            // 1. 위치 회전
            obj.RotateAround(transform.position, rotationAxis, rotationSpeed * Time.fixedDeltaTime);

            // 2. 방향 회전 (몸도 돌게 만듦)
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