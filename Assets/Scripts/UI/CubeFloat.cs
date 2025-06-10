using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFloat : MonoBehaviour
{
    public float speed = 2f;      // 움직이는 속도
    public float height = 0.5f;   // 이동 높이

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
