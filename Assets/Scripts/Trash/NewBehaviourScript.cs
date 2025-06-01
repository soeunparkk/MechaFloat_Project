using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOscillator : MonoBehaviour
{
    public float oscillateSpeed = 2f;
    public float oscillateHeight = 0.5f;

    private Vector3 initialPosition;
    private Rigidbody rb;

    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // 물리 갱신은 FixedUpdate 사용!
    {
        float offsetY = Mathf.Sin(Time.time * oscillateSpeed) * oscillateHeight;
        Vector3 targetPos = initialPosition + new Vector3(0, offsetY, 0);

        rb.MovePosition(targetPos); // 위치 이동

        // 바닥 Collider가 있다면, 이 타일은 "통과하지 않음"!
    }
}