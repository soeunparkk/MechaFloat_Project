using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private StageSO stageData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (stageData == null)
        {
            Debug.LogError("StageTrigger에 StageSO가 할당되지 않았습니다!");
            return;
        }

        MapCheckingManager.instance?.SetCurrentStage(stageData);
    }
}
