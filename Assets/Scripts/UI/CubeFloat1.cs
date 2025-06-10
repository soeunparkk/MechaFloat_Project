using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFloat1 : MonoBehaviour
{
    public float speed = 2f;      // 움직이는 속도
    public float distance = 0.5f; // 좌우 이동 거리

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newX = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPos.x + newX, startPos.y, startPos.z);
    }
}
