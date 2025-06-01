using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinApplier : MonoBehaviour
{
    [Header("Skin Root (��Ų�� ���� ��ġ)")]
    public Transform skinRoot;

    public PlayerAnimationManager animationHandler;
    public PlayerPickup playerPickup;

    // �ܺο��� ȣ��
    public void ApplySkin(SkinSO skin)
    {
        if (skin == null || skin.skinPrefab == null)
        {
            Debug.LogWarning("��Ų�� ��� �ְų� �������� �����ϴ�.");
            return;
        }

        // ���� ��Ų ����
        foreach (Transform child in skinRoot)
            Destroy(child.gameObject);

        // Instantiate + ��ġ ����
        GameObject newSkin = Instantiate(skin.skinPrefab);
        newSkin.transform.SetParent(skinRoot, false);

        newSkin.transform.localPosition = skin.skinPrefab.transform.localPosition;
        newSkin.transform.localRotation = skin.skinPrefab.transform.localRotation;
        newSkin.transform.localScale = skin.skinPrefab.transform.localScale;

        Animator skinAnimator = newSkin.GetComponentInChildren<Animator>();
        animationHandler.SetAnimator(skinAnimator);

        // Pivot ����
        Transform newPivot = FindBalloonPivot(newSkin.transform);
        playerPickup.SetBalloonPivot(newPivot);

        ApplyBalloonPrefabs(newSkin.transform);
    }

    private Transform FindBalloonPivot(Transform root)
    {
        foreach (Transform t in root.GetComponentsInChildren<Transform>())
        {
            if (t.name == "Balloon_Pivot")
                return t;
        }

        Debug.LogWarning("Balloon_Pivot not found in new skin.");
        return null;
    }

    private void ApplyBalloonPrefabs(Transform skinRoot)
    {
        foreach (Transform t in skinRoot.GetComponentsInChildren<Transform>(true))
        {
            switch (t.name)
            {
                case "Normal_Balloon":
                    PieceManager.Instance.normalBalloonObject = t.gameObject;
                    break;
                case "Helium_Balloon":
                    PieceManager.Instance.heliumBalloonObject = t.gameObject;
                    break;
                case "Reinforced_Balloon":
                    PieceManager.Instance.reinforcedBalloonObject = t.gameObject;
                    break;
            }
        }
    }
}
