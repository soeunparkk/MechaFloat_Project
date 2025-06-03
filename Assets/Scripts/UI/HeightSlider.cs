using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeightSlider : MonoBehaviour
{
    public Transform footTransform;  // 로봇 발 위치
    public Slider heightSlider;      // 세로 슬라이더
    public float minHeight = 0f;     // Stage 1 시작
    public float maxHeight = 20f;    // Stage 4 끝

    void Update()
    {
        float currentHeight = footTransform.position.y;
        float normalizedValue = Mathf.InverseLerp(minHeight, maxHeight, currentHeight);
        heightSlider.value = normalizedValue;
    }
}
