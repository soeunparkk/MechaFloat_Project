using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData
{
    public int id;
    public string stageName;
    public string description;
    public string stageNameEng;
    public float gravity;
    public string wind;
    public string difficulty;
    public string specialFeatures;
    [NonSerialized]
    public StageType stageType;
    [NonSerialized]
    public StageDifficultyType stageDifficultyType;
    public string stageTypeString;
    public string stageDifficultyTypeString;
    public bool isZeroGravityMap;

    public void InitalizeEnums()
    {
        if (Enum.TryParse(stageTypeString, out StageType parsedType))
        {
            stageType = parsedType;
        }
        else
        {
            Debug.Log($"������ : '{stageName}'�� ��ȿ���� ���� ������ Ÿ�� : {stageTypeString}");
            stageType = StageType.Ground;
        }

        if (Enum.TryParse(stageDifficultyTypeString, out StageDifficultyType difficultyType))
        {
            stageDifficultyType = difficultyType;
        }
        else
        {
            stageDifficultyType = StageDifficultyType.Easy;
        }
    }
}