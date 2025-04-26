using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TutorialData
{
    public int id;
    public string description;
    public string checkConditionTypeString;
    [NonSerialized]
    public CheckConditionType checkConditionType;
    public string parameter;
    public bool isClear;

    public void InitalizeEnums()
    {
        if (Enum.TryParse(checkConditionTypeString, out CheckConditionType parsedType))
        {
            checkConditionType = parsedType;
        }
        else
        {
            checkConditionType = CheckConditionType.InputKey;
        }
    }
}
