using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGate : MonoBehaviour
{
    public Color GateColor; // 문 고유 색

    private void OnTriggerEnter(Collider other)
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