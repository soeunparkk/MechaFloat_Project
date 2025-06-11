using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("기본 설정")]
    public GameObject meteorPrefab;
    public Transform player;
    public float spawnInterval = 3f;

    [Header("스폰 가능 구역")]
    public BoxCollider spawnArea;

    [Header("운석 스폰 허용 높이")]
    public float minSpawnHeight = 400f;
    public float maxSpawnHeight = 500f; 

    void Start()
    {
        if (meteorPrefab != null && spawnArea != null && player != null)
        {
            InvokeRepeating(nameof(SpawnMeteor), 1f, spawnInterval);
        }
        else
        {
            Debug.LogWarning("MeteorSpawner 설정이 부족합니다.");
        }
    }

    void SpawnMeteor()
    {
        float playerY = player.position.y;

        // 높이 체크
        if (playerY < minSpawnHeight || playerY > maxSpawnHeight)
            return;

        // 스폰 구역 기준으로 랜덤 위치 설정
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size * 0.8f;

        Vector3 spawnPos = new Vector3(
            Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            center.y + size.y / 2,
            Random.Range(center.z - size.z / 2, center.z + size.z / 2)
        );

        // 운석 생성
        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}
