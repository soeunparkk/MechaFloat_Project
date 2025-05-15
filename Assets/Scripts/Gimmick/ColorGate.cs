using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGate : MonoBehaviour, ICheckTrigger
{
    public Color GateColor; // �� ���� ��

    public void OnTriggerEntered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gimmick = FindObjectOfType<ColorGateGimmick>();
            if (gimmick != null)
            {
                gimmick.OnGateEntered(GateColor, other.transform);
            }
        }
    }
}