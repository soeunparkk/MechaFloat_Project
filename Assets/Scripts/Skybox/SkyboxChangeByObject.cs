using UnityEngine;

public class SkyboxChangeByObject : MonoBehaviour
{
    public Material lowerSkybox;   // ���� ������Ʈ ���Ͽ��� �� Skybox
    public Material upperSkybox;   // ���� ������Ʈ �̻󿡼� �� Skybox

    public Transform playerTransform;         // �÷��̾�(�Ǵ� ī�޶�) Transform
    public Transform triggerObjectTransform;  // ���� ���� ������Ʈ Transform

    bool isUpper = false;

    void Start()
    {
        if (playerTransform == null || triggerObjectTransform == null)
        {
            Debug.LogError("�÷��̾� �Ǵ� Ʈ���� ������Ʈ Transform�� �Ҵ���� �ʾҽ��ϴ�!");
            enabled = false;
            return;
        }
        if (lowerSkybox != null)
        {
            RenderSettings.skybox = lowerSkybox;
        }
    }

    void Update()
    {
        float playerY = playerTransform.position.y;
        float triggerY = triggerObjectTransform.position.y;

        if (!isUpper && playerY >= triggerY)
        {
            RenderSettings.skybox = upperSkybox;
            isUpper = true;
            DynamicGI.UpdateEnvironment();
        }
        else if (isUpper && playerY < triggerY)
        {
            RenderSettings.skybox = lowerSkybox;
            isUpper = false;
            DynamicGI.UpdateEnvironment();
        }
    }
}