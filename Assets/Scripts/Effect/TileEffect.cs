using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : MonoBehaviour
{
    public Material tileMaterial; // 타일 Material
    public float duration = 5f;  // 타일 유지 시간
    private float timer = 0f;
    public ParticleSystem destructionEffect; // 파괴 효과 할당

    void Update()
    {
        // 밝기 증가
        timer += Time.deltaTime;
        float emissionIntensity = Mathf.Lerp(0.5f, 2.0f, timer / duration);
        tileMaterial.SetColor("_EmissionColor", new Color(emissionIntensity, emissionIntensity, emissionIntensity));

        if (timer >= duration - 0.5f && !destructionEffect.isPlaying) // 마지막 0.5초 전에 이펙트 실행
        {
            destructionEffect.Play();
        }

        if (timer >= duration) // 시간이 다 되었을 때 타일 파괴
        {
            Destroy(gameObject); // 타일 삭제
        }
    }
}