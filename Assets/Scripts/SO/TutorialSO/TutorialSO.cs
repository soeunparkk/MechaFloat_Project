using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial", menuName = "Tutorial/Tutorial")]
public class TutorialSO : ScriptableObject
{
    public int id;
    public string description;
    public CheckConditionType checkConditionType;
    public string parameter;
    public bool isClear;
}
