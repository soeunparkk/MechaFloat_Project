using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGate : MonoBehaviour, ICheckTrigger
{
    public Color GateColor; // 문 고유 색

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