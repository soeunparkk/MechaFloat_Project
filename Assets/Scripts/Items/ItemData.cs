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

    // ���ڿ��� ���������� ��ȯ�ϴ� �޼���
    public void InitalizeEnums()
    {
        if (Enum.TryParse(itemTypeString, out ItemType parsedType))
        {
            itemType = parsedType;
        }
        else
        {
            Debug.Log($"������ : '{itemName}'�� ��ȿ���� ���� ������ Ÿ�� : {itemTypeString}");
            // �⺻�� ����
            itemType = ItemType.Balloon;
        }
    }
}
