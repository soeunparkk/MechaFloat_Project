using UnityEngine;

public class AchievementConditionChecker : MonoBehaviour
{
    private PlayerController player;
    private PlayerJump playerJump;

    private int hammerHitCount = 0;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        playerJump = GetComponent<PlayerJump>();
    }

    // 300m 도달 업적
    public void CheckHeightAchievement()
    {
        if (player.transform.position.y >= -7f)
        {
            AchievementManager.Instance.TryUnlockAchievement(8); // 진격의 로봇
        }
    }

    // 점프 업적
    public void OnJumpPerformed()
    {
        if (playerJump.jumpCount >= 30)
            AchievementManager.Instance.TryUnlockAchievement(4); // 초보 점퍼

        if (playerJump.jumpCount >= 50)
            AchievementManager.Instance.TryUnlockAchievement(14); // 끝없는 점프
    }

    // =============================================================================================
    // 점차 추가 예정

    // 추락했을 때 호출
    /*public void OnFall()
    {
        if (playerJump.fallCount >= 5)
            AchievementManager.Instance.TryUnlockAchievement(1); // 태초마을의 주민
    }*/

    // 망치랑 충돌했을 때 호출
    /*public void OnHammerHit()
    {
        hammerHitCount++;

        if (hammerHitCount >= 4)
            AchievementManager.Instance.TryUnlockAchievement(5); // 충돌 마스터
    }*/
}
