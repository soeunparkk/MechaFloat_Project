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
        SaveManager.Instance.SetStartPosition(startPosition.transform.position); // ���� ��ġ�� ���� ��ġ�� ����

        if (SkinSelectorData.selectedSkin != null)
        {
            playerSkinApplier.ApplySkin(SkinSelectorData.selectedSkin);
        }
        else
        {
            Debug.LogWarning("���õ� ��Ų�� �����ϴ�.");
        }
    }
}
