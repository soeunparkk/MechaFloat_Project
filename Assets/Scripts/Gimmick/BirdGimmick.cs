using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGimmick : MonoBehaviour, ICheckTrigger
{
    private PlayerDie playerDie;

    private void Start()
    {
        playerDie = FindObjectOfType<PlayerDie>();
    }

    public void OnTriggerEntered(Collider other)
    {
        if (other.CompareTag("Player") && playerDie != null)
        {
            playerDie.Die();
        }
    }
}
