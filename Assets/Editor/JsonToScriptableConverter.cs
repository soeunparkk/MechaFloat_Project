#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public enum ConversionType
{
    Items,
    Stages,
    Objects,
    Achievement,
    Skin,
    Title
}

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePath = "";                                               // JSON ���� ��η� ���ڿ� ��
    private string outputFolder = "Assets/ScriptableObjects";                       // ��� SO ������ ��� ��
    private bool creatDatabase = true;                                              // ������ ���̽��� ��� �� �������� ���� bool ��
    private ConversionType conversionType = ConversionType.Items;

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

        // ��ȯ Ÿ�� ����
        conversionType = (ConversionType)EditorGUILayout.EnumPopup("Conversion Type: ", conversionType);

        // Ÿ�Կ� ���� �⺻ ��� ���� ����
        if (conversionType == ConversionType.Items)
        {
            outputFolder = "Assets/ScriptableObjects/Items";
        }
        else if (conversionType == ConversionType.Stages)
        {
            outputFolder = "Assets/ScriptableObjects/Stages";
        }
        else if (conversionType == ConversionType.Objects)
        {
            outputFolder = "Assets/ScriptableObjects/Objects";
        }
        else if (conversionType == ConversionType.Achievement)
        {
            outputFolder = "Assets/ScriptableObjects/Achievements";
        }
        else if (conversionType == ConversionType.Skin)
        {
            outputFolder = "Assets/ScriptableObjects/Skins";
        }
        else if (conversionType == ConversionType.Title)
        {
            outputFolder = "Assets/ScriptableObjects/Titles";
        }

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
            switch (conversionType)
            {
                case ConversionType.Items:
                    ConverterJsonToItemScriptableObjects();
                    break;

                case ConversionType.Stages:
                    ConverterJsonToStageScriptableObjects();
                    break;
                case ConversionType.Objects:
                    ConverterJsonToObjectScriptableObjects();
                    break;
                case ConversionType.Achievement:
                    ConverterJsonToAchievementScriptableObjects();
                    break;
                case ConversionType.Skin:
                    ConverterJsonToSkinScriptableObjects();
                    break;
                case ConversionType.Title:
                    ConverterJsonToTitleScriptableObjects();
                    break;
            }
        }
    }

    private void ConverterJsonToItemScriptableObjects()                                 // JSON ������ ScriptableObject ���Ϸ� ��ȯ �����ִ� �Լ�
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

                itemSO.maxHP = itemData.maxHP;
                itemSO.isBuoyancy = itemData.isBuoyancy;
                itemSO.isReinforced = itemData.isReinforced;
                itemSO.buoyancyForce = itemData.buoyancyForce;
                itemSO.degradationRate = itemData.degradationRate;
                itemSO.durabilityMultiplier = itemData.durabilityMultiplier;
                itemSO.gravityScale = itemData.gravityScale;
                itemSO.maxRiseSpeed = itemData.maxRiseSpeed;
                itemSO.maxFallSpeed = itemData.maxFallSpeed;
                

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

    private void ConverterJsonToStageScriptableObjects()
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
            List<StageData> stageDataList = JsonConvert.DeserializeObject<List<StageData>>(jsonText);

            List<StageSO> createdStages = new List<StageSO>();

            foreach (var stageData in stageDataList)
            {
                StageSO stageSO = ScriptableObject.CreateInstance<StageSO>();

                // ������ ����
                stageSO.id = stageData.id;
                stageSO.stageName = stageData.stageName;
                stageSO.stageNameEng = stageData.stageNameEng;
                stageSO.description = stageData.description;

                // ������ ��ȯ
                if (System.Enum.TryParse(stageData.stageTypeString, out StageType parsedType))
                {
                    stageSO.stageType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"������ '{stageData.stageName}'�� ��ȿ���� ���� Ÿ�� : {stageData.stageTypeString}");
                }

                stageSO.gravity = stageData.gravity;
                stageSO.wind = stageData.wind;
                stageSO.difficulty = stageData.difficulty;
                stageSO.specialFeatures = stageData.specialFeatures;
                stageSO.isZeroGravityMap = stageData.isZeroGravityMap;

                // ��ũ���ͺ� ������Ʈ ���� - ID�� 4�ڸ� ���ڷ� ������
                string assetPath = $"{outputFolder}/Stage_{stageData.id.ToString("D4")}_{stageData.stageNameEng}.asset";
                AssetDatabase.CreateAsset(stageSO, assetPath);

                // ���� �̸� ����
                stageSO.name = $"Stage_{stageData.id.ToString("D4")}+{stageData.stageNameEng}";
                createdStages.Add(stageSO);

                EditorUtility.SetDirty(stageSO);
            }

            if (creatDatabase && createdStages.Count > 0)
            {
                StageDatabaseSO database = ScriptableObject.CreateInstance<StageDatabaseSO>();
                database.stages = createdStages;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/StageDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdStages.Count} scriptable objects!", "OK");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ����: . {e}");
        }
    }

    private void ConverterJsonToObjectScriptableObjects()
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
            List<ObjectData> objectDataList = JsonConvert.DeserializeObject<List<ObjectData>>(jsonText);

            List<ObjectSO> createdobjects = new List<ObjectSO>();

            foreach (var objectData in objectDataList)
            {
                ObjectSO objectSO = ScriptableObject.CreateInstance<ObjectSO>();

                // ������ ����
                objectSO.id = objectData.id;
                objectSO.objectName = objectData.objectName;
                objectSO.objectNameEng = objectData.objectNameEng;
                objectSO.description = objectData.description;

                // ������ ��ȯ
                if (System.Enum.TryParse(objectData.objectTypeString, out ObjectType parsedType))
                {
                    objectSO.objectType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"������ '{objectData.objectName}'�� ��ȿ���� ���� Ÿ�� : {objectData.objectTypeString}");
                }

                objectSO.stageAppearanceeffect = objectData.stageAppearanceeffect;
                objectSO.StatusEffect = objectData.StatusEffect;

                // ��ũ���ͺ� ������Ʈ ���� - ID�� 4�ڸ� ���ڷ� ������
                string assetPath = $"{outputFolder}/Object_{objectData.id.ToString("D4")}_{objectData.objectNameEng}.asset";
                AssetDatabase.CreateAsset(objectSO, assetPath);

                // ���� �̸� ����
                objectSO.name = $"Object_{objectData.id.ToString("D4")}+{objectData.objectNameEng}";
                createdobjects.Add(objectSO);

                EditorUtility.SetDirty(objectSO);
            }

            if (creatDatabase && createdobjects.Count > 0)
            {
                ObjectDatabaseSO database = ScriptableObject.CreateInstance<ObjectDatabaseSO>();
                database.objects = createdobjects;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/StageDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdobjects.Count} scriptable objects!", "OK");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ����: . {e}");
        }
    }

    private void ConverterJsonToAchievementScriptableObjects()
    {
        if (!Directory.Exists(outputFolder))                                     
        {
            Directory.CreateDirectory(outputFolder);
        }

        // JSON ���� �б�
        string jsonText = File.ReadAllText(jsonFilePath);                          

        try
        {
            List<AchievementData> achievementDataList = JsonConvert.DeserializeObject<List<AchievementData>>(jsonText);

            List<AchievementSO> createdAchievements = new List<AchievementSO>();             

            foreach (var achievementData in achievementDataList)
            {
                AchievementSO achievementSO = ScriptableObject.CreateInstance<AchievementSO>();

                // ������ ����
                achievementSO.id = achievementData.id;
                achievementSO.achievementName = achievementData.achievementName;
                achievementSO.description = achievementData.description;
                achievementSO.isAchievement = achievementData.isAchievement;
                achievementSO.compensationSkin = achievementData.compensationSkin;
                achievementSO.compensationTitle = achievementData.compensationTitle;

                string assetPath = $"{outputFolder}/Item_{achievementData.id.ToString("D4")}_{achievementData.achievementName}.asset";
                AssetDatabase.CreateAsset(achievementSO, assetPath);

                achievementSO.name = $"Item_{achievementData.id.ToString("D4")}+{achievementData.achievementName}";
                createdAchievements.Add(achievementSO);

                EditorUtility.SetDirty(achievementSO);
            }

            // �����ͺ��̽� ����
            if (creatDatabase && createdAchievements.Count > 0)
            {
                AchievementDatabaseSO database = ScriptableObject.CreateInstance<AchievementDatabaseSO>();
                database.achievements = createdAchievements;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/AchievementDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdAchievements.Count} scriptable objects!", "OK");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ����: . {e}");
        }
    }

    private void ConverterJsonToSkinScriptableObjects()
    {
        if (!Directory.Exists(outputFolder))                                    
        {
            Directory.CreateDirectory(outputFolder);
        }

        string jsonText = File.ReadAllText(jsonFilePath);                       

        try
        {
            List<SkinData> skinDataList = JsonConvert.DeserializeObject<List<SkinData>>(jsonText);

            List<SkinSO> createdSkins = new List<SkinSO>();                         

            foreach (var skinData in skinDataList)
            {
                SkinSO skinSO = ScriptableObject.CreateInstance<SkinSO>();

                skinSO.id = skinData.id;
                skinSO.skinName = skinData.skinName;

                if (!string.IsNullOrEmpty(skinData.skinPath))
                {
                    skinSO.skin = AssetDatabase.LoadAssetAtPath<Mesh>($"Assests/Resources/{skinData.skinPath}.png");

                    if (skinSO.skin == null)
                    {
                        Debug.LogWarning($"������ '{skinData.skinName}'�� �������� ã�� �� �����ϴ�. : {skinData.skinPath}");
                    }
                }

                string assetPath = $"{outputFolder}/Item_{skinData.id.ToString("D4")}_{skinData.skinName}.asset";
                AssetDatabase.CreateAsset(skinSO, assetPath);

                skinSO.name = $"Item_{skinData.id.ToString("D4")}+{skinData.skinName}";
                createdSkins.Add(skinSO);

                EditorUtility.SetDirty(skinSO);
            }

            if (creatDatabase && createdSkins.Count > 0)
            {
                SkinDatabaseSO database = ScriptableObject.CreateInstance<SkinDatabaseSO>();      
                database.skins = createdSkins;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/SkinDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdSkins.Count} scriptable objects!", "OK");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ����: . {e}");
        }
    }

    private void ConverterJsonToTitleScriptableObjects()
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        string jsonText = File.ReadAllText(jsonFilePath);

        try
        {
            List<TitleData> titleDataList = JsonConvert.DeserializeObject<List<TitleData>>(jsonText);

            List<TitleSO> createdTitles = new List<TitleSO>();

            foreach (var titleData in titleDataList)
            {
                TitleSO titleSO = ScriptableObject.CreateInstance<TitleSO>();

                titleSO.id = titleData.id;
                titleSO.titleName = titleData.titleName;

                if (!string.IsNullOrEmpty(titleData.titlePath))
                {
                    titleSO.title = AssetDatabase.LoadAssetAtPath<Sprite>($"Assests/Resources/{titleData.titlePath}.png");

                    if (titleSO.title == null)
                    {
                        Debug.LogWarning($"������ '{titleData.titleName}'�� �������� ã�� �� �����ϴ�. : {titleData.titlePath}");
                    }
                }

                string assetPath = $"{outputFolder}/Item_{titleData.id.ToString("D4")}_{titleData.titleName}.asset";
                AssetDatabase.CreateAsset(titleSO, assetPath);

                titleSO.name = $"Item_{titleData.id.ToString("D4")}+{titleData.titleName}";
                createdTitles.Add(titleSO);

                EditorUtility.SetDirty(titleSO);
            }

            if (creatDatabase && createdTitles.Count > 0)
            {
                TitleDatabaseSO database = ScriptableObject.CreateInstance<TitleDatabaseSO>();
                database.titles = createdTitles;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/TitleDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdTitles.Count} scriptable objects!", "OK");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON : {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ����: . {e}");
        }
    }
}

#endif