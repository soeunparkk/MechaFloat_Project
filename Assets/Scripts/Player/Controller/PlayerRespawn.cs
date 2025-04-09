using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private PlayerPickup playerPickup;

    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
    }

    public void RespawnPlayer()
    {
        if (SaveManager.Instance.TryGetSavedPosition(out Vector3 respawnPos))
        {
            playerTransform.position = respawnPos;
            RestoreBalloonDurability();
            Debug.Log("부활 위치로 이동 및 풍선 내구도 복구 완료");
        }
        else
        {
            Debug.LogWarning("세이브된 위치가 없습니다.");
        }
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
