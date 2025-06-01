using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinApplier : MonoBehaviour
{
    [Header("Skin Root (스킨이 붙을 위치)")]
    public Transform skinRoot;

    public PlayerAnimationManager animationHandler;
    public PlayerPickup playerPickup;

    // 외부에서 호출
    public void ApplySkin(SkinSO skin)
    {
        if (skin == null || skin.skinPrefab == null)
        {
            Debug.LogWarning("스킨이 비어 있거나 프리팹이 없습니다.");
            return;
        }

        // 기존 스킨 제거
        foreach (Transform child in skinRoot)
            Destroy(child.gameObject);

        // Instantiate + 위치 유지
        GameObject newSkin = Instantiate(skin.skinPrefab);
        newSkin.transform.SetParent(skinRoot, false);

        newSkin.transform.localPosition = skin.skinPrefab.transform.localPosition;
        newSkin.transform.localRotation = skin.skinPrefab.transform.localRotation;
        newSkin.transform.localScale = skin.skinPrefab.transform.localScale;

        Animator skinAnimator = newSkin.GetComponentInChildren<Animator>();
        animationHandler.SetAnimator(skinAnimator);

        // Pivot 연결
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
