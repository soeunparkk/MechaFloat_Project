using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGateGimmick : MonoBehaviour
{
    [SerializeField] private List<Color> colorSequence = new();  // ����ؾ� �� �� ����
    [SerializeField] private List<ColorGate> gates;              // �ش� ����� ����
    [SerializeField] private int colorCount = 5;                 // ����� �� ����
    public int currentIndex;

    private void Start()
    {
        currentIndex = 0;
    }

    public void OnGateEntered(Color gateColor)
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
            PlayerRespawn playerRespawn = FindObjectOfType<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.RespawnPlayer();
            }

            currentIndex = 0;
        }
    }
}
