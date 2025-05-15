using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    public void RespawnPlayer()
    {
        Vector3 respawnPos = SaveManager.Instance.GetRespawnPosition();

        playerTransform.position = respawnPos;
        RestoreBalloonDurability();

        Debug.Log("부활 완료: 위치 이동 + 풍선 내구도 복구");
    }

    private void RestoreBalloonDurability()
    {
        for (int i = 0; i < 4; i++)
        {
            BalloonController balloon = InventoryManager.Instance.GetBalloonAtSlot(i);
            if (balloon != null)
            {
                balloon.SetCurrentDurability(balloon.balloonData.maxHP);
                InventoryManager.Instance.UpdateHotbarUI(i);
            }
        }
    }
}
