using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDatabase", menuName = "StageDatabase/Database")]
public class StageDatabaseSO : ScriptableObject
{
    public List<StageSO> stages = new List<StageSO>();

    private Dictionary<int, StageSO> stagesById;
    private Dictionary<string, StageSO> stagesByName;

    public void Initialize()
    {
        stagesById = new Dictionary<int, StageSO>();
        stagesByName = new Dictionary<string, StageSO>();

        foreach (var stage in stages)
        {
            stagesById[stage.id] = stage;
            stagesByName[stage.stageNameEng] = stage;
        }
    }

    public StageSO GetItemById(int id)
    {
        if (stagesById == null)                                      
        {
            Initialize();
        }

        if (stagesById.TryGetValue(id, out StageSO stage))
            return stage;                                                

        return null;                                             
    }

    public StageSO GetItemByName(string name)
    {
        if (stagesByName == null)                                     
        {
            Initialize();
        }

        if (stagesByName.TryGetValue(name, out StageSO stage))           
            return stage;

        return null;
    }

    public List<StageSO> GetItemByType(StageType type)
    {
        return stages.FindAll(stage => stage.stageType == type);
    }
}