using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGateGimmick : MonoBehaviour
{
    [SerializeField] private List<Color> colorSequence = new();  // 기억해야 할 색 순서
    [SerializeField] private List<ColorGate> gates;              // 해당 기믹의 문들
    [SerializeField] private int colorCount = 5;                 // 기억할 색 개수
    [SerializeField] private float knockBackForce = 20f;

    private int currentIndex;

    private void Start()
    {
        currentIndex = 0;
    }

    public void OnGateEntered(Color gateColor, Transform playerTransform)
    {
        if (gateColor == colorSequence[currentIndex])
        {
            currentIndex++;
            if (currentIndex >= colorSequence.Count)
            {
                Debug.Log("기믹 성공!");
                // 완료 처리
            }
        }
        else
        {
            // 튕겨내기
            PlayerController controller = playerTransform.GetComponent<PlayerController>();
            if (controller != null)
            {
                Vector3 knockbackDir = (playerTransform.position - transform.position).normalized;
                controller.Knockback(knockbackDir + Vector3.back, knockBackForce);
            }

            currentIndex = 0;
        }
    }
}
