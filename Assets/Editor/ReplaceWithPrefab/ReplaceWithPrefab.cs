using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefab : EditorWindow
{
    private GameObject prefab;

    [MenuItem("Tools/Replace With Prefab")]
    static void Init()
    {
        ReplaceWithPrefab window = (ReplaceWithPrefab)EditorWindow.GetWindow(typeof(ReplaceWithPrefab));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("�� ������ ����", EditorStyles.boldLabel);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("�����׸��� ���������� ��� ��ü"))
        {
            Replace();
        }
    }

    void Replace()
    {
        if (prefab == null || Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("�����հ� ��� ������Ʈ�� ��� �������ּ���.");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.scene.IsValid())
            {
                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, obj.transform.parent);
                newObj.transform.position = obj.transform.position;
                newObj.transform.rotation = obj.transform.rotation;
                newObj.transform.localScale = obj.transform.localScale;

                Undo.RegisterCreatedObjectUndo(newObj, "Replace With Prefab");
                Undo.DestroyObjectImmediate(obj); // ���� ������Ʈ ���� (��� ����)
            }
        }
    }
}
