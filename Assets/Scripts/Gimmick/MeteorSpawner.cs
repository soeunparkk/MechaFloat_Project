using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;      // 운석 프리팹
    public Transform player;             // 플레이어 위치
    public float spawnRadius = 30f;      // 플레이어 주변 몇 m 반경에 떨어질지
    public float spawnInterval = 3f;     // 몇 초마다 운석 생성

    void Start()
    {
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnInterval);
    }

    void SpawnMeteor()
    {
        if (meteorPrefab == null || player == null)
            return;

        // 플레이어 주변에서 랜덤 위치 생성
        Vector3 spawnPos = player.position + Random.onUnitSphere * spawnRadius;
        spawnPos.y = player.position.y + 10f; // 위에서 떨어뜨리기

        GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        // 방향으로 힘 주는 건 Meteor.cs에서 알아서 처리
    }
}
