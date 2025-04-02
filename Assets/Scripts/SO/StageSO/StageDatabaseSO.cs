using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDatabase", menuName = "StageDatabase/Database")]
public class StageDatabaseSO : ScriptableObject
{
    public List<StageSO> stages = new List<StageSO>();             // StageSO를 리스트로 관리 한다.

    // 캐싱을 위한 사전
    private Dictionary<int, StageSO> stagesById;                  // ID로 아이템 찾기 위한 캐싱
    private Dictionary<string, StageSO> stagesByName;             // 이름으로 아이템 찾기

    public void Initialize()
    {
        stagesById = new Dictionary<int, StageSO>();              // 위에 선언만 했기 때문에 Dictionary 할당
        stagesByName = new Dictionary<string, StageSO>();

        foreach (var stage in stages)                             // Items 리스트에 선언 되어 있는것을 가지고 Dictionary에 입력한다.
        {
            stagesById[stage.id] = stage;
            stagesByName[stage.stageName] = stage;
        }
    }

    // ID로 아이템 찾기
    public StageSO GetItemById(int id)
    {
        if (stagesById == null)                                          // ItemById 가 캐싱이 되어 있지 않다면 초기화 한다.
        {
            Initialize();
        }

        if (stagesById.TryGetValue(id, out StageSO stage))
            return stage;                                                // ID 값을 통해서 ItemSO를 리턴 한다.

        return null;                                                    // 없을 경우 NULL
    }

    // 이름으로 아이템 찾기
    public StageSO GetItemByName(string name)
    {
        if (stagesByName == null)                                        // ItemsByName 가 캐싱이 되어 있지 않다면 초기화 한다.
        {
            Initialize();
        }

        if (stagesByName.TryGetValue(name, out StageSO stage))             // Name 값을 통해서 ItemSO를 리턴 한다.
            return stage;

        return null;
    }

    // 타입으로 아이템 필터링
    public List<StageSO> GetItemByType(StageType type)
    {
        return stages.FindAll(stage => stage.stageType == type);
    }
}