using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEventTrigger : MonoBehaviour
{
    public string triggerEventName;

    private TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager?.TriggerEvent(triggerEventName);
            gameObject.SetActive(false);
        }
    }
}
