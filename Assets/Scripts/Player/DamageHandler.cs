using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    private PlayerPickup playerPickup;
    private BalloonController balloonController;

    private void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
        balloonController = GetComponent<BalloonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
        {
            if (playerPickup != null && playerPickup.equippedBalloon != null)
            {
                playerPickup.equippedBalloon.TakeDamage(5f);
            }
            else if (balloonController != null)
            {
                balloonController.TakeDamage(5f);
            }
        }
    }
}