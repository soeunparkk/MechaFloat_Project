using UnityEngine;

public class DamageHandler : MonoBehaviour, ICheckTrigger
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }
    }

    public void OnTriggerEntered(Collider other)
    {
        if (playerController != null && playerController.IsInvincible)
        {
            Debug.Log("무적 상태이므로 피해 무시됨");
            return;
        }

        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
        {
            BalloonController equippedBalloon = InventoryManager.Instance.GetSelectedBalloon();
            if (equippedBalloon != null)
            {
                equippedBalloon.TakeDamage(5f);
                InventoryManager.Instance.UpdateHotbarUI(equippedBalloon.assignedSlot);
            }
        }
    }
}