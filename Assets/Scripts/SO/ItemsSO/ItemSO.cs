using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public int id;                                          // ���� ID��  
    public string itemName;                                 // �̸�
    public string nameEng;                                  // ���� �̸�
    public string description;                              // ����
    public ItemType itemType;                               // ������ Ÿ��
    public int maxHP;                                       // �ִ� ������
    public bool isBuoyancy;                                 // �η� ���� bool ��
    public bool isReinforced;                               // ��ȭ ǳ�� ���� bool ��
    public Sprite icon;                                     // �̹��� ������
    public GameObject prefab;
    public float buoyancyForce;                             // �η½� �߷� ������
    public int degradationRate;                             // �ʴ� ������ ��� ��
    public float durabilityMultiplier;                      // ��ȭ ǳ���� ������ ȿ��
    public float gravityScale;                              // �η� ����� ����� �߷� ������
    public float maxRiseSpeed;                              // �ִ� ��� �ӵ�
    public float maxFallSpeed;                              // �ִ� �ϰ� �ӵ�

    public override string ToString()
    {
        return $"[{id}] {itemName} ({itemType}) - HP";
    }

    public string DisplayName
    {
        get { return string.IsNullOrEmpty(nameEng) ? itemName : nameEng; }
    }
}