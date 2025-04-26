using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBreakEffect : MonoBehaviour
{
    public GameObject breakEffectPrefab;  // ����Ʈ ������ ������ ��

    public void PlayBreakEffect()
    {
        // ���� ��ġ�� ��ƼŬ ����Ʈ�� ���� ����
        Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
    }
}

