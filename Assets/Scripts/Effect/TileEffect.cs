using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : MonoBehaviour
{
    public Material tileMaterial; // Ÿ�� Material
    public float duration = 5f;  // Ÿ�� ���� �ð�
    private float timer = 0f;
    public ParticleSystem destructionEffect; // �ı� ȿ�� �Ҵ�

    void Update()
    {
        // ��� ����
        timer += Time.deltaTime;
        float emissionIntensity = Mathf.Lerp(0.5f, 2.0f, timer / duration);
        tileMaterial.SetColor("_EmissionColor", new Color(emissionIntensity, emissionIntensity, emissionIntensity));

        if (timer >= duration - 0.5f && !destructionEffect.isPlaying) // ������ 0.5�� ���� ����Ʈ ����
        {
            destructionEffect.Play();
        }

        if (timer >= duration) // �ð��� �� �Ǿ��� �� Ÿ�� �ı�
        {
            Destroy(gameObject); // Ÿ�� ����
        }
    }
}