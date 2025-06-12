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

    // � ���� ����
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            SpawnMeteor(); // ��� �� �� ����
            InvokeRepeating(nameof(SpawnMeteor), spawnInterval, spawnInterval);
        }
    }

    // � ���� ����
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

       

        // �÷��̾� �����θ� � ���� ��ġ ���� (�÷��̾� ���� 10~30m �Ÿ�, �ణ ���� ��ġ)
        Vector3 forward = player.forward.normalized;
        float distanceAhead = Random.Range(10f, 30f);
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)); // �¿� �ణ ����

        Vector3 spawnPos = player.position + forward * distanceAhead + randomOffset;
        spawnPos.y = player.position.y + 10f; // ������ ����߸���

        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}