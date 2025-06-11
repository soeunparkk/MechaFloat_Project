using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaxHeight : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI maxHeightText;

    private float maxHeight = 0f;
    private float startY = 0f; // 기준점 (시작 y값)

    void Start()
    {
        // 시작 시 플레이어의 y좌표를 기준으로 저장
        startY = player.position.y;
        maxHeight = 0f;
        maxHeightText.text = $"최고 높이: {maxHeight:F2}m";
    }

    void Update()
    {
        // 현재 위치에서 시작 지점을 뺀 상대 높이
        float relativeY = player.position.y - startY;

        // 최고 높이 갱신
        if (relativeY > maxHeight)
        {
            maxHeight = relativeY;
        }

        maxHeightText.text = $"최고 높이: {maxHeight:F2}m";
    }
}
