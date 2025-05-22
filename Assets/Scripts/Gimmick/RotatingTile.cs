using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTile : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // 회전축 (기본은 Y축)
    public float rotationSpeed = 45f; // 초당 회전 속도 (도 단위)

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
