using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementDatabase", menuName = "Achievement/Database")]
public class AchievementDatabaseSO : ScriptableObject
{
    public List<AchievementSO> achievements = new List<AchievementSO>();

    private Dictionary<int, AchievementSO> achievementById;
    private Dictionary<string, AchievementSO> achievementByName;

    public void Initialize()
    {
        achievementById = new Dictionary<int, AchievementSO>();
        achievementByName = new Dictionary<string, AchievementSO>();

        foreach (var achievement in achievements)
        {
            achievementById[achievement.id] = achievement;
            achievementByName[achievement.achievementName] = achievement;
        }
    }

    // ID로 아이템 찾기
    public AchievementSO GetItemById(int id)
    {
        if (achievementById == null)   
        {
            Initialize();
        }

        if (achievementById.TryGetValue(id, out AchievementSO achievement))
            return achievement;

        return null;
    }

    // 이름으로 아이템 찾기
    public AchievementSO GetItemByName(string name)
    {
        if (achievementByName == null)
        {
            Initialize();
        }

        if (achievementByName.TryGetValue(name, out AchievementSO achievement))
            return achievement;

        return null;
    }
}
