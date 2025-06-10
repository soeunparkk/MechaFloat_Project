using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;      // � ������
    public Transform player;             // �÷��̾� ��ġ
    public float spawnRadius = 30f;      // �÷��̾� �ֺ� �� m �ݰ濡 ��������
    public float spawnInterval = 3f;     // �� �ʸ��� � ����

    void Start()
    {
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnInterval);
    }

    void SpawnMeteor()
    {
        if (meteorPrefab == null || player == null)
            return;

        // �÷��̾� �ֺ����� ���� ��ġ ����
        Vector3 spawnPos = player.position + Random.onUnitSphere * spawnRadius;
        spawnPos.y = player.position.y + 10f; // ������ ����߸���

        GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        // �������� �� �ִ� �� Meteor.cs���� �˾Ƽ� ó��
    }
}
