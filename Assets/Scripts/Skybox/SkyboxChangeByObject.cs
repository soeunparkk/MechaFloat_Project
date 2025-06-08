using UnityEngine;

public class SkyboxChangeByObject : MonoBehaviour
{
    public Material lowerSkybox;   // 기준 오브젝트 이하에서 쓸 Skybox
    public Material upperSkybox;   // 기준 오브젝트 이상에서 쓸 Skybox

    public Transform playerTransform;         // 플레이어(또는 카메라) Transform
    public Transform triggerObjectTransform;  // 기준 높이 오브젝트 Transform

    bool isUpper = false;

    void Start()
    {
        if (playerTransform == null || triggerObjectTransform == null)
        {
            Debug.LogError("플레이어 또는 트리거 오브젝트 Transform이 할당되지 않았습니다!");
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