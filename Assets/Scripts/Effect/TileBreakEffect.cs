using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBreakEffect : MonoBehaviour
{
    public GameObject breakEffectPrefab;  // 이펙트 프리팹 연결할 곳

    public void PlayBreakEffect()
    {
        // 현재 위치에 파티클 이펙트를 새로 생성
        Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
    }
}

