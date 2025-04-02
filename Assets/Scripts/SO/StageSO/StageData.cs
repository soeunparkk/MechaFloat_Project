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

    // 문자열을 열거형으로 변환하는 메서드
    public void InitalizeEnums()
    {
        if (Enum.TryParse(stageTypeString, out StageType parsedType))
        {
            stageType = parsedType;
        }
        else
        {
            Debug.Log($"아이템 : '{stageName}'에 유효하지 않은 아이템 타입 : {stageTypeString}");
            // 기본값 설정
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