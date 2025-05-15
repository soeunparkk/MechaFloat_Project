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
            FindObjectOfType<ColorGateGimmick>().OnGateEntered(GateColor);
        }
    }
}