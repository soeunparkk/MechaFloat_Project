using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    public void RespawnPlayer()
    {
        Vector3 respawnPos = SaveManager.Instance.GetRespawnPosition();

        playerTransform.position = respawnPos;
        RestoreBalloonDurability();

        Debug.Log("��Ȱ �Ϸ�: ��ġ �̵� + ǳ�� ������ ����");
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
