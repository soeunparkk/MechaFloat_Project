#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePath = "";                                               // JSON ���� ��η� ���ڿ� ��
    private string outputFolder = "Assets/ScriptableObjects/Items";                 // ��� SO ������ ��� ��
    private bool creatDatabase = true;                                              // ������ ���̽��� ��� �� �������� ���� bool ��

    [MenuItem("Tools/JSON to Scriptable Objects")]

    public static void ShowIndow()
    {
        GetWindow<JsonToScriptableConverter>("JOSN to Scriptable Objects");
    }

    void OnGUI()
    {
        GUILayout.Label("JSON to Scriptable Object Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Select JSON FIle"))
        {
            jsonFilePath = EditorUtility.OpenFilePanel("Select JSON FIle", "", "json");
        }

        EditorGUILayout.LabelField("Selectd File : ", jsonFilePath);
        EditorGUILayout.Space();
        outputFolder = EditorGUILayout.TextField("Output Folder :", outputFolder);
        creatDatabase = EditorGUILayout.Toggle("Create Database Asset", creatDatabase);
        EditorGUILayout.Space();

        if (GUILayout.Button("Convert to Scriptable Objects"))
        {
            if (string.IsNullOrEmpty(jsonFilePath))
            {
                EditorUtility.DisplayDialog("Error", "Please select a JSON file firest!", "OK");
                return;
            }
            ConverterJsonToScriptableObjects();
        }
    }

    private void ConverterJsonToScriptableObjects()                                 // JSON ������ ScriptableObject ���Ϸ� ��ȯ �����ִ� �Լ�
    {
        // ���� ����
        if (!Directory.Exists(outputFolder))                                        // ���� ��ġ�� Ȯ���ϰ� ������ ���� �Ѵ�.
        {
            Directory.CreateDirectory(outputFolder);
        }

        // JSON ���� �б�
        string jsonText = File.ReadAllText(jsonFilePath);                           // JSON ������ �д´�

        try
        {
            // JSON �Ľ�
            List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(jsonText);

            List<ItemSO> createdItems = new List<ItemSO>();                         // ItemSO ����Ʈ ����

            // �� ������ �����͸� ��ũ���ͺ� ������Ʈ�� ��ȯ
            foreach (var itemData in itemDataList)
            {
                ItemSO itemSO = ScriptableObject.CreateInstance<ItemSO>();          // ItemSO ������ ����

                // ������ ����
                itemSO.id = itemData.id;
                itemSO.itemName = itemData.itemName;
                itemSO.nameEng = itemData.nameEng;
                itemSO.description = itemData.description;

                // ������ ��ȯ
                if (System.Enum.TryParse(itemData.itemTypeString, out ItemType parsedType))
                {
                    itemSO.itemType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"������ '{itemData.itemName}'�� ��ȿ���� ���� Ÿ�� : {itemData.itemTypeString}");
                }

                // ��ȹ�ڰ� ǳ�� ���� ��Ʈ �ָ� �װſ� �°� �� �߰��� ����
                itemSO.HP = itemData.HP;
                itemSO.isBuoyancy = itemData.isBuoyancy;

                // ������ �ε� (��ΰ� �ִ� ���)
                if (!string.IsNullOrEmpty(itemData.iconPath))
                {
                    itemSO.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assests/Resources/{itemData.iconPath}.png");

                    if (itemSO.icon == null)
                    {
                        Debug.LogWarning($"������ '{itemData.nameEng}'�� �������� ã�� �� �����ϴ�. : {itemData.iconPath}");
                    }
                }

                // ��ũ���ͺ� ������Ʈ ���� - ID�� 4�ڸ� ���ڷ� ������
                string assetPath = $"{outputFolder}/Item_{itemData.id.ToString("D4")}_{itemData.nameEng}.asset";
                AssetDatabase.CreateAsset(itemSO, assetPath);

                // ���� �̸� ����
                itemSO.name = $"Item_{itemData.id.ToString("D4")}+{itemData.nameEng}";
                createdItems.Add(itemSO);

                EditorUtility.SetDirty(itemSO);
            }

            // �����ͺ��̽� ����
            if (creatDatabase && createdItems.Count > 0)
            {
                ItemDatabaseSO database = ScriptableObject.CreateInstance<ItemDatabaseSO>();        // ItemDatabaseSO ����
                database.items = createdItems;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/ItemDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdItems.Count} scriptable objects!", "OK");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ����: . {e}");
        }
    }
}

#endif