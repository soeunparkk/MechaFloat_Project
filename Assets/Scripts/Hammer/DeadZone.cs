using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour, ICheckTrigger
{
    [SerializeField] private float delayBeforeDeath = 2f;

    public void OnTriggerEntered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDie playerDie = other.GetComponent<PlayerDie>();
            if (playerDie != null)
            {
                StartCoroutine(DelayedDeath(playerDie));
            }
        }
    }

    private IEnumerator DelayedDeath(PlayerDie playerDie)
    {
        yield return new WaitForSeconds(delayBeforeDeath);
        playerDie.Die();
    }
}
