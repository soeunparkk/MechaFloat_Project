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
        GUILayout.Label("새 프리팹 선택", EditorStyles.boldLabel);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("선택항목을 프리팹으로 모두 교체"))
        {
            Replace();
        }
    }

    void Replace()
    {
        if (prefab == null || Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("프리팹과 대상 오브젝트를 모두 선택해주세요.");
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
                Undo.DestroyObjectImmediate(obj); // 기존 오브젝트 삭제 (취소 가능)
            }
        }
    }
}
