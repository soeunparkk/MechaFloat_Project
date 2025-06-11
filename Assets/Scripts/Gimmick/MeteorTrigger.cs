using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorTrigger : MonoBehaviour
{
    private MeteorSpawner spawner;

    private void Awake()
    {
        spawner = FindObjectOfType<MeteorSpawner>();
        if (spawner == null)
            Debug.LogWarning("MeteorSpawner not found in scene!");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger enter: {other.name}");
        if (other.CompareTag("Player") && spawner != null)
        {
            Debug.Log("Player entered meteor trigger tile");
            spawner.StartSpawning();
        }
    }
}
