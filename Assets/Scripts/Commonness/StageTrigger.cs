using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    [SerializeField] private StageSO stageData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (stageData == null)
        {
            Debug.LogError("StageTrigger�� StageSO�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        MapCheckingManager.instance?.SetCurrentStage(stageData);
    }
}
