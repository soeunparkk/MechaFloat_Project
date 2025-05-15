using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    public void RespawnPlayer()
    {
        Vector3 respawnPos = SaveManager.Instance.GetRespawnPosition();

        playerTransform.position = respawnPos;

        Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        RestoreBalloonDurability();
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
