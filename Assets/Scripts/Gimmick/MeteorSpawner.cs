using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("�⺻ ����")]
    public GameObject meteorPrefab;
    public Transform player;
    public float spawnInterval = 3f;

    [Header("���� ���� ����")]
    public BoxCollider spawnArea;

    [Header("� ���� ��� ����")]
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
            Debug.LogWarning("MeteorSpawner ������ �����մϴ�.");
        }
    }

    void SpawnMeteor()
    {
        float playerY = player.position.y;

        // ���� üũ
        if (playerY < minSpawnHeight || playerY > maxSpawnHeight)
            return;

        // ���� ���� �������� ���� ��ġ ����
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size * 0.8f;

        Vector3 spawnPos = new Vector3(
            Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            center.y + size.y / 2,
            Random.Range(center.z - size.z / 2, center.z + size.z / 2)
        );

        // � ����
        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }
}
