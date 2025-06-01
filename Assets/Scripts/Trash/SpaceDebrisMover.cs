using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풍선에 달 수 있는 간단한 Damage 처리 (원하는대로 커스텀 가능!)
public class Balloon : MonoBehaviour
{
    public int hp = 3;

    public void Damage(int amount)
    {
        hp -= amount;
        Debug.Log("풍선 데미지! 현재 HP: " + hp);
        if (hp <= 0)
        {
            Destroy(gameObject); // 터짐
        }
    }
}

// 우주쓰레기 이동 + 충돌 체크 한번에!
public class SpaceDebris : MonoBehaviour
{
    public float moveSpeed = 1.0f; // 이동 속도
    public Vector3 moveDirection;   // 이동 방향

    void Start()
    {
        // Rigidbody가 있으면 무중력으로 (use gravity 끄기)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = false;

        // 방향 자동 랜덤 지정
        if (moveDirection == Vector3.zero)
            moveDirection = Random.onUnitSphere;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        // Player(혹은 풍선)의 태그를 "Player"로 설정했다고 가정
        if (other.CompareTag("Player"))
        {
            // 풍선 스크립트 컴포넌트 찾아서 데미지 처리
            Balloon balloon = other.GetComponent<Balloon>();
            if (balloon != null)
            {
                balloon.Damage(1);
            }
            // 이 쓰레기 오브젝트 제거(옵션)
            // Destroy(gameObject);
        }
    }
}