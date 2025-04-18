using UnityEngine;

public class AchievementConditionChecker : MonoBehaviour
{
    private PlayerController player;
    private PlayerJump playerJump;

    private float maxHeightReached = 0f;
    private int hammerHitCount = 0;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        playerJump = GetComponent<PlayerJump>();
    }

    // 300m 도달 업적
    public void CheckHeightAchievement(float currentHeight)
    {
        if (currentHeight > maxHeightReached)
        {
            maxHeightReached = currentHeight;

            if (maxHeightReached >= 300f)
            { 
                AchievementManager.Instance.TryUnlockAchievement(8);
            }
        }
    }

    // 점프 업적
    public void OnJumpPerformed(int jumpCount)
    {
        if (jumpCount >= 30)
            AchievementManager.Instance.TryUnlockAchievement(4);

        if (jumpCount >= 50)
            AchievementManager.Instance.TryUnlockAchievement(14);
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
