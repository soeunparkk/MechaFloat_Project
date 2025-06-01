using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public int id;                                          // 고유 ID값  
    public string itemName;                                 // 이름
    public string nameEng;                                  // 영어 이름
    public string description;                              // 설명
    public ItemType itemType;                               // 아이템 타입
    public int maxHP;                                       // 최대 내구도
    public bool isBuoyancy;                                 // 부력 여부 bool 값
    public bool isReinforced;                               // 강화 풍선 여부 bool 값
    public Sprite icon;                                     // 이미지 아이콘
    public GameObject prefab;
    public float buoyancyForce;                             // 부력시 중력 증가량
    public int degradationRate;                             // 초당 내구도 닳는 양
    public float durabilityMultiplier;                      // 강화 풍선의 내구도 효율
    public float gravityScale;                              // 부력 적용시 사용할 중력 스케일
    public float maxRiseSpeed;                              // 최대 상승 속도
    public float maxFallSpeed;                              // 최대 하강 속도

    public override string ToString()
    {
        return $"[{id}] {itemName} ({itemType}) - HP";
    }

    public string DisplayName
    {
        get { return string.IsNullOrEmpty(nameEng) ? itemName : nameEng; }
    }
}