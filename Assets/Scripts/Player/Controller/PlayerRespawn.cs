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
            Debug.Log("��Ȱ ��ġ�� �̵� �� ǳ�� ������ ���� �Ϸ�");
        }
        else
        {
            Debug.LogWarning("���̺�� ��ġ�� �����ϴ�.");
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
