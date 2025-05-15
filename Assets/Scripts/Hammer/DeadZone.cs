using UnityEngine;

public class DeadZone : MonoBehaviour, ICheckTrigger
{
    public void OnTriggerEntered(Collider other)
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
