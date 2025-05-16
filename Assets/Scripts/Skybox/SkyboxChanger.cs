using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material defaultSkybox;
    public Material spaceSkybox;

    public float changeHeight = 500f; // 이 높이 넘으면 스카이박스 바뀜

    private bool isSpace = false;

    void Update()
    {
        float playerHeight = transform.position.y;

        if (!isSpace && playerHeight >= changeHeight)
        {
            RenderSettings.skybox = spaceSkybox;
            DynamicGI.UpdateEnvironment();
            isSpace = true;
        }
        else if (isSpace && playerHeight < changeHeight)
        {
            RenderSettings.skybox = defaultSkybox;
            DynamicGI.UpdateEnvironment();
            isSpace = false;
        }
    }
}

