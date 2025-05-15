using UnityEngine;

public class SomeTriggerObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent<ICheckTrigger>(out var triggerHandler))
        {
            triggerHandler.OnTriggerEntered(other);
        }
    }
}
