using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [Header("업적 데이터베이스")]
    public AchievementDatabaseSO achievementDatabase;
    public SkinDatabaseSO skinDatabase;
    public TitleDatabaseSO titleDatabase;

    [Header("업적 UI 연동")]
    public AchievementPopupUI popupUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 데이터베이스 초기화
        achievementDatabase.Initialize();
        skinDatabase.Initialize();
        titleDatabase.Initialize();
    }

    // 업적 달성 체크 (업적 ID로)
    public void TryUnlockAchievement(int achievementId)
    {
        AchievementSO achievement = achievementDatabase.GetItemById(achievementId);
        if (achievement == null)
        {
            Debug.LogWarning($"업적 ID {achievementId}를 찾을 수 없습니다.");
            return;
        }

        if (achievement.isAchievement)
        {
            Debug.Log($"이미 달성된 업적: {achievement.achievementName}");
            return;
        }

        // 달성 처리
        achievement.isAchievement = true;

        Debug.Log($"업적 달성! : {achievement.achievementName}");

        // 보상 지급
        GrantReward(achievement);

        // 업적 달성 UI 호출
        popupUI.ShowPopup(achievement);
    }

    // 보상 지급 처리
    private void GrantReward(AchievementSO achievement)
    {
        // 스킨
        SkinSO skin = skinDatabase.GetItemById(achievement.compensationSkin);
        if (skin != null)
        {
            Debug.Log($"스킨 획득: {skin.skinName}");
            // 인벤토리 등록 or 추가 가능
        }

        // 칭호
        TitleSO title = titleDatabase.GetItemById(achievement.compensationTitle);
        if (title != null)
        {
            Debug.Log($"칭호 획득: {title.titleName}");
            // 칭호 시스템에 등록
        }
    }
}
