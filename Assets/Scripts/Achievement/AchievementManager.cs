using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [Header("���� �����ͺ��̽�")]
    public AchievementDatabaseSO achievementDatabase;
    public SkinDatabaseSO skinDatabase;
    public TitleDatabaseSO titleDatabase;

    [Header("���� UI ����")]
    public AchievementPopupUI popupUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // �����ͺ��̽� �ʱ�ȭ
        achievementDatabase.Initialize();
        skinDatabase.Initialize();
        titleDatabase.Initialize();
    }

    // ���� �޼� üũ (���� ID��)
    public void TryUnlockAchievement(int achievementId)
    {
        AchievementSO achievement = achievementDatabase.GetItemById(achievementId);
        if (achievement == null)
        {
            Debug.LogWarning($"���� ID {achievementId}�� ã�� �� �����ϴ�.");
            return;
        }

        if (achievement.isAchievement)
        {
            Debug.Log($"�̹� �޼��� ����: {achievement.achievementName}");
            return;
        }

        // �޼� ó��
        achievement.isAchievement = true;

        Debug.Log($"���� �޼�! : {achievement.achievementName}");

        // ���� ����
        GrantReward(achievement);

        // ���� �޼� UI ȣ��
        popupUI.ShowPopup(achievement);
    }

    // ���� ���� ó��
    private void GrantReward(AchievementSO achievement)
    {
        // ��Ų
        SkinSO skin = skinDatabase.GetItemById(achievement.compensationSkin);
        if (skin != null)
        {
            Debug.Log($"��Ų ȹ��: {skin.skinName}");
            // �κ��丮 ��� or �߰� ����
        }

        // Īȣ
        TitleSO title = titleDatabase.GetItemById(achievement.compensationTitle);
        if (title != null)
        {
            Debug.Log($"Īȣ ȹ��: {title.titleName}");
            // Īȣ �ý��ۿ� ���
        }
    }
}
