using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "ObjectDatabase/Object")]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectSO> objects = new List<ObjectSO>();

    // ĳ���� ���� ����
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

    // ID�� ������ ã��
    public ObjectSO GetItemById(int id)
    {
        if (objsetsById == null)                                          // ItemById �� ĳ���� �Ǿ� ���� �ʴٸ� �ʱ�ȭ �Ѵ�.
        {
            Initialize();
        }

        if (objsetsById.TryGetValue(id, out ObjectSO _object))
            return _object;                                                // ID ���� ���ؼ� ItemSO�� ���� �Ѵ�.

        return null;                                                    // ���� ��� NULL
    }

    // �̸����� ������ ã��
    public ObjectSO GetItemByName(string name)
    {
        if (objsetsByName == null)                                        // ItemsByName �� ĳ���� �Ǿ� ���� �ʴٸ� �ʱ�ȭ �Ѵ�.
        {
            Initialize();
        }

        if (objsetsByName.TryGetValue(name, out ObjectSO _object))             // Name ���� ���ؼ� ItemSO�� ���� �Ѵ�.
            return _object;

        return null;
    }

    // Ÿ������ ������ ���͸�
    public List<ObjectSO> GetItemByType(ObjectType type)
    {
        return objects.FindAll(_object => _object.objectType == type);
    }
}
