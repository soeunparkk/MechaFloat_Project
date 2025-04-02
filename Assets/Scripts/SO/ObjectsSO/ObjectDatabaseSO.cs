using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "ObjectDatabase/Object")]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectSO> objects = new List<ObjectSO>();

    // 캐싱을 위한 사전
    private Dictionary<int, ObjectSO> objsetsById;
    private Dictionary<string, ObjectSO> objsetsByName;

    public void Initialize()
    {
        objsetsById = new Dictionary<int, ObjectSO>();
        objsetsByName = new Dictionary<string, ObjectSO>();

        foreach (var _object in objects)
        {
            objsetsById[_object.id] = _object;
            objsetsByName[_object.objectName] = _object;
        }
    }

    // ID로 아이템 찾기
    public ObjectSO GetItemById(int id)
    {
        if (objsetsById == null)                                          // ItemById 가 캐싱이 되어 있지 않다면 초기화 한다.
        {
            Initialize();
        }

        if (objsetsById.TryGetValue(id, out ObjectSO _object))
            return _object;                                                // ID 값을 통해서 ItemSO를 리턴 한다.

        return null;                                                    // 없을 경우 NULL
    }

    // 이름으로 아이템 찾기
    public ObjectSO GetItemByName(string name)
    {
        if (objsetsByName == null)                                        // ItemsByName 가 캐싱이 되어 있지 않다면 초기화 한다.
        {
            Initialize();
        }

        if (objsetsByName.TryGetValue(name, out ObjectSO _object))             // Name 값을 통해서 ItemSO를 리턴 한다.
            return _object;

        return null;
    }

    // 타입으로 아이템 필터링
    public List<ObjectSO> GetItemByType(ObjectType type)
    {
        return objects.FindAll(_object => _object.objectType == type);
    }
}
