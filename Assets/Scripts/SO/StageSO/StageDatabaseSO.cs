using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDatabase", menuName = "StageDatabase/Database")]
public class StageDatabaseSO : ScriptableObject
{
    public List<StageSO> stages = new List<StageSO>();             // StageSO�� ����Ʈ�� ���� �Ѵ�.

    // ĳ���� ���� ����
    private Dictionary<int, StageSO> stagesById;                  // ID�� ������ ã�� ���� ĳ��
    private Dictionary<string, StageSO> stagesByName;             // �̸����� ������ ã��

    public void Initialize()
    {
        stagesById = new Dictionary<int, StageSO>();              // ���� ���� �߱� ������ Dictionary �Ҵ�
        stagesByName = new Dictionary<string, StageSO>();

        foreach (var stage in stages)                             // Items ����Ʈ�� ���� �Ǿ� �ִ°��� ������ Dictionary�� �Է��Ѵ�.
        {
            stagesById[stage.id] = stage;
            stagesByName[stage.stageName] = stage;
        }
    }

    // ID�� ������ ã��
    public StageSO GetItemById(int id)
    {
        if (stagesById == null)                                          // ItemById �� ĳ���� �Ǿ� ���� �ʴٸ� �ʱ�ȭ �Ѵ�.
        {
            Initialize();
        }

        if (stagesById.TryGetValue(id, out StageSO stage))
            return stage;                                                // ID ���� ���ؼ� ItemSO�� ���� �Ѵ�.

        return null;                                                    // ���� ��� NULL
    }

    // �̸����� ������ ã��
    public StageSO GetItemByName(string name)
    {
        if (stagesByName == null)                                        // ItemsByName �� ĳ���� �Ǿ� ���� �ʴٸ� �ʱ�ȭ �Ѵ�.
        {
            Initialize();
        }

        if (stagesByName.TryGetValue(name, out StageSO stage))             // Name ���� ���ؼ� ItemSO�� ���� �Ѵ�.
            return stage;

        return null;
    }

    // Ÿ������ ������ ���͸�
    public List<StageSO> GetItemByType(StageType type)
    {
        return stages.FindAll(stage => stage.stageType == type);
    }
}