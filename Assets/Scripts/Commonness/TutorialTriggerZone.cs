using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerZone : MonoBehaviour
{
    public int nextTutorialId;

    private TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager?.GoToTutorialStep(nextTutorialId);
            gameObject.SetActive(false);
        }
    }
}
