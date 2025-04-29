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
        // 플레이어의 시작 높이로 초기화
        maxHeight = player.position.y;
    }

    void Update()
    {
        float currentY = player.position.y;

        if (currentY > maxHeight)
        {
            maxHeight = currentY;
        }

        // 최고 높이 계속 표시 (조건문 밖으로 이동)
        maxHeightText.text = $"최고 높이: {maxHeight:F2}m";
    }

}
