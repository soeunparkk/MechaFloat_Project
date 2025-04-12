using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
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