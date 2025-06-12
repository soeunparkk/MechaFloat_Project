using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Transform player;
    public float spawnRadius = 30f;
    public float spawnInterval = 3f;

    private bool isSpawning = false;

    // 운석 스폰 시작
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            SpawnMeteor(); // 즉시 한 번 생성
            InvokeRepeating(nameof(SpawnMeteor), spawnInterval, spawnInterval);
        }
    }

    // 운석 스폰 멈춤
    public void StopSpawning()
    {
        if (isSpawning)
        {
            isSpawning = false;
            CancelInvoke(nameof(SpawnMeteor));
        }
    }

    void SpawnMeteor()
    {
        
        if (meteorPrefab == null || player == null) return;

       

        // 플레이어 앞으로만 운석 생성 위치 지정 (플레이어 전방 10~30m 거리, 약간 랜덤 위치)
        Vector3 forward = player.forward.normalized;
        float distanceAhead = Random.Range(10f, 30f);
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)); // 좌우 약간 랜덤

        Vector3 spawnPos = player.position + forward * distanceAhead + randomOffset;
        spawnPos.y = player.position.y + 10f; // 위에서 떨어뜨리기

        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}