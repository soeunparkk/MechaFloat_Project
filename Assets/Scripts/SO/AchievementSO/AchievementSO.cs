using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement/Achievement")]
public class AchievementSO : ScriptableObject
{
    public int id;
    public string achievementName;
    public string description;
    public bool isAchievement;
    public int compensationSkin;
    public int compensationTitle;

    public override string ToString()
    {
        return $"[{id}] {achievementName}";
    }
}
