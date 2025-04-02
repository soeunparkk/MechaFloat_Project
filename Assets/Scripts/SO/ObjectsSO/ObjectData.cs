using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectData : MonoBehaviour
{
    public int id;
    public string objectName;
    public string description;
    public string objectNameEng;
    public string stageAppearanceeffect;
    public string StatusEffect;
    [NonSerialized]
    public ObjectType objectType;
    public string objectTypeString;

    public void InitalizeEnums()
    {
        if (Enum.TryParse(objectTypeString, out ObjectType parsedType))
        {
            objectType = parsedType;
        }
        else
        {
            Debug.Log($"아이템 : '{objectName}'에 유효하지 않은 아이템 타입 : {objectTypeString}");
            objectType = ObjectType.Rock;
        }
    }
}
