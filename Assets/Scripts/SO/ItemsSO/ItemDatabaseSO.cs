using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();             // ItemSO�� ����Ʈ�� ���� �Ѵ�.

    // ĳ���� ���� ����
    private Dictionary<int, ItemSO> itemsById;                  // ID�� ������ ã�� ���� ĳ��
    private Dictionary<string, ItemSO> itemsByName;             // �̸����� ������ ã��
    private Dictionary<ItemType, GameObject> balloonPrefabMap;

    public void Initialize()
    {
        itemsById = new Dictionary<int, ItemSO>();              // ���� ���� �߱� ������ Dictionary �Ҵ�
        itemsByName = new Dictionary<string, ItemSO>();
        balloonPrefabMap = new Dictionary<ItemType, GameObject>();

        foreach (var item in items)                             // Items ����Ʈ�� ���� �Ǿ� �ִ°��� ������ Dictionary�� �Է��Ѵ�.
        {
            itemsById[item.id] = item;
            itemsByName[item.itemName] = item;
        }

        foreach (var item in items)
        {
            balloonPrefabMap[item.itemType] = item.prefab;
        }
    }

    // ID�� ������ ã��
    public ItemSO GetItemById(int id)
    {
        if (itemsById == null)                                          // ItemById �� ĳ���� �Ǿ� ���� �ʴٸ� �ʱ�ȭ �Ѵ�.
        {
            Initialize();
        }

        if (itemsById.TryGetValue(id, out ItemSO item))
            return item;                                                // ID ���� ���ؼ� ItemSO�� ���� �Ѵ�.

        return null;                                                    // ���� ��� NULL
    }

    // �̸����� ������ ã��
    public ItemSO GetItemByName(string name)
    {
        if (itemsByName == null)                                        // ItemsByName �� ĳ���� �Ǿ� ���� �ʴٸ� �ʱ�ȭ �Ѵ�.
        {
            Initialize();
        }

        if (itemsByName.TryGetValue(name, out ItemSO item))             // Name ���� ���ؼ� ItemSO�� ���� �Ѵ�.
            return item;

        return null;
    }

    // Ÿ������ ������ ���͸�
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