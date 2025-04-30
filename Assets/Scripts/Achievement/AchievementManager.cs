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

        // 보상 가져오기
        SkinSO skin = skinDatabase.GetItemById(achievement.compensationSkin);
        TitleSO title = titleDatabase.GetItemById(achievement.compensationTitle);

        // 보상 지급 처리
        GrantReward(skin, title);

        // 업적 팝업 UI 호출
        popupUI.ShowPopup(achievement, skin, title);
    }

    // 보상 지급 처리
    private void GrantReward(SkinSO skin, TitleSO title)
    {
        if (skin != null)
        {
            Debug.Log($"스킨 획득: {skin.skinName}");
        }

        if (title != null)
        {
            Debug.Log($"칭호 획득: {title.titleName}");
        }
    }
}
