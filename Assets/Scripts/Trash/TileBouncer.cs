using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBouncer : MonoBehaviour
{
    public float oscillateSpeed = 2f;    // 움직이는 속도
    public float oscillateHeight = 0.5f; // 이동 높이
    public float minY = 0f;              // y의 최저 한계값(바닥 위치)

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * oscillateSpeed) * oscillateHeight;
        float targetY = initialPosition.y + offsetY;

        // 바닥 이하로 안 내려가게 제한
        if (targetY < minY)
            targetY = minY;

        transform.position = new Vector3(initialPosition.x, targetY, initialPosition.z);
    }
}