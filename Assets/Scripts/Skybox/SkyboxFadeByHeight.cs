using UnityEngine;

public class SkyboxFadeByHeight : MonoBehaviour
{
    public Material fadeSkyboxMaterial; // Inspector���� BlendSkybox ��Ƽ���� �Ҵ�
    public Transform playerTransform;  // �÷��̾ ���� ������Ʈ
    public float fadeStartHeight = 50.0f; // ���̵� ���� ����
    public float fadeEndHeight = 60.0f;   // ���̵� 1�� �Ǵ�(���� ��ȯ) ����

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