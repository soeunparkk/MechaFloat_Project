using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private PlayerPickup playerPickup;

    [SerializeField] private Transform playerTransform;

    void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
    }

    public void RespawnPlayer()
    {
        Vector3 respawnPos = SaveManager.Instance.GetRespawnPosition();

        playerTransform.position = respawnPos;
        RestoreBalloonDurability();

        Debug.Log("부활 완료: 위치 이동 + 풍선 내구도 복구");
    }

    private void RestoreBalloonDurability()
    {
        if (playerPickup.equippedBalloon != null)
        {
            ItemSO balloonData = playerPickup.equippedBalloon.balloonData;
            playerPickup.equippedBalloon.SetCurrentDurability(balloonData.maxHP);
        }
    }
}
