using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();             // ItemSO를 리스트로 관리 한다.

    // 캐싱을 위한 사전
    private Dictionary<int, ItemSO> itemsById;                  // ID로 아이템 찾기 위한 캐싱
    private Dictionary<string, ItemSO> itemsByName;             // 이름으로 아이템 찾기
    private Dictionary<ItemType, GameObject> balloonPrefabMap;

    public void Initialize()
    {
        itemsById = new Dictionary<int, ItemSO>();              // 위에 선언만 했기 때문에 Dictionary 할당
        itemsByName = new Dictionary<string, ItemSO>();
        balloonPrefabMap = new Dictionary<ItemType, GameObject>();

        foreach (var item in items)                             // Items 리스트에 선언 되어 있는것을 가지고 Dictionary에 입력한다.
        {
            itemsById[item.id] = item;
            itemsByName[item.itemName] = item;
        }

        foreach (var item in items)
        {
            balloonPrefabMap[item.itemType] = item.prefab;
        }
    }

    // ID로 아이템 찾기
    public ItemSO GetItemById(int id)
    {
        if (itemsById == null)                                          // ItemById 가 캐싱이 되어 있지 않다면 초기화 한다.
        {
            Initialize();
        }

        if (itemsById.TryGetValue(id, out ItemSO item))
            return item;                                                // ID 값을 통해서 ItemSO를 리턴 한다.

        return null;                                                    // 없을 경우 NULL
    }

    // 이름으로 아이템 찾기
    public ItemSO GetItemByName(string name)
    {
        if (itemsByName == null)                                        // ItemsByName 가 캐싱이 되어 있지 않다면 초기화 한다.
        {
            Initialize();
        }

        if (itemsByName.TryGetValue(name, out ItemSO item))             // Name 값을 통해서 ItemSO를 리턴 한다.
            return item;

        return null;
    }

    // 타입으로 아이템 필터링
    public List<ItemSO> GetItemByType(ItemType type)
    {
        return items.FindAll(item => item.itemType == type);
    }

    public GameObject GetBalloonPrefab(ItemType balloonType)
    {
        if (balloonPrefabMap == null) Initialize();
        return balloonPrefabMap.TryGetValue(balloonType, out var prefab) ? prefab : null;
    }
}