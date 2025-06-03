using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeightSlider : MonoBehaviour
{
    public Transform footTransform;  // �κ� �� ��ġ
    public Slider heightSlider;      // ���� �����̴�
    public float minHeight = 0f;     // Stage 1 ����
    public float maxHeight = 20f;    // Stage 4 ��

    void Update()
    {
        float currentHeight = footTransform.position.y;
        float normalizedValue = Mathf.InverseLerp(minHeight, maxHeight, currentHeight);
        heightSlider.value = normalizedValue;
    }
}
