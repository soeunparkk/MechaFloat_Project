using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaxHeight : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI maxHeightText;

    private float maxHeight;

    void Start()
    {
        // �÷��̾��� ���� ���̷� �ʱ�ȭ
        maxHeight = player.position.y;
    }

    void Update()
    {
        float currentY = player.position.y;

        if (currentY > maxHeight)
        {
            maxHeight = currentY;
        }

        // �ְ� ���� ��� ǥ�� (���ǹ� ������ �̵�)
        maxHeightText.text = $"�ְ� ����: {maxHeight:F2}m";
    }

}
