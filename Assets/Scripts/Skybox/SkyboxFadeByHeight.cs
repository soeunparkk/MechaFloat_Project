using UnityEngine;

public class SkyboxFadeByHeight : MonoBehaviour
{
    public Material fadeSkyboxMaterial; // Inspector에서 BlendSkybox 머티리얼 할당
    public Transform playerTransform;  // 플레이어나 기준 오브젝트
    public float fadeStartHeight = 50.0f; // 페이드 시작 높이
    public float fadeEndHeight = 60.0f;   // 페이드 1이 되는(완전 전환) 높이

    void Start()
    {
        if (fadeSkyboxMaterial != null)
        {
            RenderSettings.skybox = fadeSkyboxMaterial;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;
        float y = playerTransform.position.y;
        float t = Mathf.InverseLerp(fadeStartHeight, fadeEndHeight, y); // 0~1
        fadeSkyboxMaterial.SetFloat("_Blend", t);
        DynamicGI.UpdateEnvironment();
    }
}