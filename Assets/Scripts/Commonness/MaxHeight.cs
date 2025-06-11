using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaxHeight : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI maxHeightText;

    private float maxHeight = 0f;
    private float startY = 0f; // ������ (���� y��)

    void Start()
    {
        // ���� �� �÷��̾��� y��ǥ�� �������� ����
        startY = player.position.y;
        maxHeight = 0f;
        maxHeightText.text = $"�ְ� ����: {maxHeight:F2}m";
    }

    void Update()
    {
        // ���� ��ġ���� ���� ������ �� ��� ����
        float relativeY = player.position.y - startY;

        // �ְ� ���� ����
        if (relativeY > maxHeight)
        {
            maxHeight = relativeY;
        }

        maxHeightText.text = $"�ְ� ����: {maxHeight:F2}m";
    }
}
