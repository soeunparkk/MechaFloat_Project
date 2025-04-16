using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDie playerDie = other.GetComponent<PlayerDie>();
            if (playerDie != null)
            {
                playerDie.Die();
            }
        }
    }
}
