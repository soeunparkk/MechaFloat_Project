using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "StageDatabase/Stage")]
public class StageSO : ScriptableObject
{
    public int id;
    public string stageName;
    public string description;
    public string stageNameEng;
    public float gravity;
    public string wind;
    public string difficulty;
    public string specialFeatures;
    public StageType stageType;
    public StageDifficultyType stageDifficultyType;
    public bool isZeroGravityMap;

    public string DisplayName
    {
        get { return string.IsNullOrEmpty(stageNameEng) ? stageName : stageNameEng; }
    }
}