using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform startPosition;

    public PlayerSkinApplier playerSkinApplier;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        SaveManager.Instance.SetStartPosition(startPosition.transform.position); // 현재 위치를 시작 위치로 설정

        if (SkinSelectorData.selectedSkin != null)
        {
            playerSkinApplier.ApplySkin(SkinSelectorData.selectedSkin);
        }
        else
        {
            Debug.LogWarning("선택된 스킨이 없습니다.");
        }
    }
}
