using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public int id;
    public string itemName;
    public string description;
    public string nameEng;
    public string itemTypeString;
    [NonSerialized]
    public ItemType itemType;
    public int HP;
    public bool isBuoyancy;
    public string iconPath;

    // 문자열을 열거형으로 변환하는 메서드
    public void InitalizeEnums()
    {
        if (Enum.TryParse(itemTypeString, out ItemType parsedType))
        {
            itemType = parsedType;
        }
        else
        {
            Debug.Log($"아이템 : '{itemName}'에 유효하지 않은 아이템 타입 : {itemTypeString}");
            // 기본값 설정
            itemType = ItemType.Balloon;
        }
    }
}
