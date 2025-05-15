using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGateGimmick : MonoBehaviour
{
    [SerializeField] private List<Color> colorSequence = new();  // ����ؾ� �� �� ����
    [SerializeField] private List<ColorGate> gates;              // �ش� ����� ����
    [SerializeField] private int colorCount = 5;                 // ����� �� ����
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
                Debug.Log("��� ����!");
                // �Ϸ� ó��
            }
        }
        else
        {
            // ƨ�ܳ���
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
