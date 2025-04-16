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

    // 300m ���� ����
    public void CheckHeightAchievement()
    {
        if (player.transform.position.y >= -7f)
        {
            AchievementManager.Instance.TryUnlockAchievement(8); // ������ �κ�
        }
    }

    // ���� ����
    public void OnJumpPerformed()
    {
        if (playerJump.jumpCount >= 30)
            AchievementManager.Instance.TryUnlockAchievement(4); // �ʺ� ����

        if (playerJump.jumpCount >= 50)
            AchievementManager.Instance.TryUnlockAchievement(14); // ������ ����
    }

    // =============================================================================================
    // ���� �߰� ����

    // �߶����� �� ȣ��
    /*public void OnFall()
    {
        if (playerJump.fallCount >= 5)
            AchievementManager.Instance.TryUnlockAchievement(1); // ���ʸ����� �ֹ�
    }*/

    // ��ġ�� �浹���� �� ȣ��
    /*public void OnHammerHit()
    {
        hammerHitCount++;

        if (hammerHitCount >= 4)
            AchievementManager.Instance.TryUnlockAchievement(5); // �浹 ������
    }*/
}
