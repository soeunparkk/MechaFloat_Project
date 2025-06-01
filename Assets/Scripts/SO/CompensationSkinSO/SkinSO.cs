using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Compensation_Skin", menuName = "Compensation/Skin")]
public class SkinSO : ScriptableObject
{
    public int id;
    public string skinName;
    public Sprite skinImage;
    public GameObject skinPrefab;

    public AchievementSO unlockCondition;
}
