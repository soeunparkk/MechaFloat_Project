using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPatrol : MonoBehaviour
{
    public float patrolDistance = 5f;  // 왔다갔다할 거리(총 길이)
    public float speed = 2f;           // 이동 속도

    private Vector3 startPos;
    private int dir = 1;               // 방향: 1(오른쪽), -1(왼쪽)

    void Start()
    {
        startPos = transform.position; // 시작 위치 저장
    }

    void Update()
    {
        // 이동
        transform.Translate(Vector2.right * dir * speed * Time.deltaTime);

        // 경계 체크(패트롤 범위 초과시 방향 반전)
        if (Mathf.Abs(transform.position.x - startPos.x) > patrolDistance * 0.5f)
        {
            dir *= -1; // 방향 반전
            // 새의 모양(스프라이트)이 좌우 뒤집혀야 한다면 아래 추가
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}

