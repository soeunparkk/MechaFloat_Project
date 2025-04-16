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
    private string jsonFilePath = "";                                               // JSON 파일 경로로 문자열 값
    private string outputFolder = "Assets/ScriptableObjects";                       // 출력 SO 파일을 경로 값
    private bool creatDatabase = true;                                              // 데이터 베이스를 사용 할 것인지에 대한 bool 값
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

        // 변환 타입 선택
        conversionType = (ConversionType)EditorGUILayout.EnumPopup("Conversion Type: ", conversionType);

        // 타입에 따라 기본 출력 폴더 설정
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

    private void ConverterJsonToItemScriptableObjects()                                 // JSON 파일을 ScriptableObject 파일로 변환 시켜주는 함수
    {
        // 폴더 생성
        if (!Directory.Exists(outputFolder))                                        // 폴더 위치를 확인하고 없으면 생성 한다.
        {
            Directory.CreateDirectory(outputFolder);
        }

        // JSON 파일 읽기
        string jsonText = File.ReadAllText(jsonFilePath);                           // JSON 파일을 읽는다

        try
        {
            // JSON 파싱
            List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(jsonText);

            List<ItemSO> createdItems = new List<ItemSO>();                         // ItemSO 리스트 생성

            // 각 아이템 데이터를 스크립터블 오브젝트로 변환
            foreach (var itemData in itemDataList)
            {
                ItemSO itemSO = ScriptableObject.CreateInstance<ItemSO>();          // ItemSO 파일을 생성

                // 데이터 복사
                itemSO.id = itemData.id;
                itemSO.itemName = itemData.itemName;
                itemSO.nameEng = itemData.nameEng;
                itemSO.description = itemData.description;

                // 열거형 변환
                if (System.Enum.TryParse(itemData.itemTypeString, out ItemType parsedType))
                {
                    itemSO.itemType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"아이템 '{itemData.itemName}'의 유효하지 않은 타입 : {itemData.itemTypeString}");
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
                

                // 아이콘 로드 (경로가 있는 경우)
                if (!string.IsNullOrEmpty(itemData.iconPath))
                {
                    itemSO.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assests/Resources/{itemData.iconPath}.png");

                    if (itemSO.icon == null)
                    {
                        Debug.LogWarning($"아이템 '{itemData.nameEng}'의 아이콘을 찾을 수 없습니다. : {itemData.iconPath}");
                    }
                }

                // 스크립터블 오브젝트 저장 - ID를 4자리 수자로 포맷팅
                string assetPath = $"{outputFolder}/Item_{itemData.id.ToString("D4")}_{itemData.nameEng}.asset";
                AssetDatabase.CreateAsset(itemSO, assetPath);

                // 에셋 이름 지정
                itemSO.name = $"Item_{itemData.id.ToString("D4")}+{itemData.nameEng}";
                createdItems.Add(itemSO);

                EditorUtility.SetDirty(itemSO);
            }

            // 데이터베이스 생성
            if (creatDatabase && createdItems.Count > 0)
            {
                ItemDatabaseSO database = ScriptableObject.CreateInstance<ItemDatabaseSO>();        // ItemDatabaseSO 생성
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
            Debug.LogError($"JSON 변환 오류: . {e}");
        }
    }

    private void ConverterJsonToStageScriptableObjects()
    {
        // 폴더 생성
        if (!Directory.Exists(outputFolder))                                        // 폴더 위치를 확인하고 없으면 생성 한다.
        {
            Directory.CreateDirectory(outputFolder);
        }

        // JSON 파일 읽기
        string jsonText = File.ReadAllText(jsonFilePath);                           // JSON 파일을 읽는다

        try
        {
            List<StageData> stageDataList = JsonConvert.DeserializeObject<List<StageData>>(jsonText);

            List<StageSO> createdStages = new List<StageSO>();

            foreach (var stageData in stageDataList)
            {
                StageSO stageSO = ScriptableObject.CreateInstance<StageSO>();

                // 데이터 복사
                stageSO.id = stageData.id;
                stageSO.stageName = stageData.stageName;
                stageSO.stageNameEng = stageData.stageNameEng;
                stageSO.description = stageData.description;

                // 열거형 변환
                if (System.Enum.TryParse(stageData.stageTypeString, out StageType parsedType))
                {
                    stageSO.stageType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"아이템 '{stageData.stageName}'의 유효하지 않은 타입 : {stageData.stageTypeString}");
                }

                stageSO.gravity = stageData.gravity;
                stageSO.wind = stageData.wind;
                stageSO.difficulty = stageData.difficulty;
                stageSO.specialFeatures = stageData.specialFeatures;
                stageSO.isZeroGravityMap = stageData.isZeroGravityMap;

                // 스크립터블 오브젝트 저장 - ID를 4자리 수자로 포맷팅
                string assetPath = $"{outputFolder}/Stage_{stageData.id.ToString("D4")}_{stageData.stageNameEng}.asset";
                AssetDatabase.CreateAsset(stageSO, assetPath);

                // 에셋 이름 지정
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
            Debug.LogError($"JSON 변환 오류: . {e}");
        }
    }

    private void ConverterJsonToObjectScriptableObjects()
    {
        // 폴더 생성
        if (!Directory.Exists(outputFolder))                                        // 폴더 위치를 확인하고 없으면 생성 한다.
        {
            Directory.CreateDirectory(outputFolder);
        }

        // JSON 파일 읽기
        string jsonText = File.ReadAllText(jsonFilePath);                           // JSON 파일을 읽는다

        try
        {
            List<ObjectData> objectDataList = JsonConvert.DeserializeObject<List<ObjectData>>(jsonText);

            List<ObjectSO> createdobjects = new List<ObjectSO>();

            foreach (var objectData in objectDataList)
            {
                ObjectSO objectSO = ScriptableObject.CreateInstance<ObjectSO>();

                // 데이터 복사
                objectSO.id = objectData.id;
                objectSO.objectName = objectData.objectName;
                objectSO.objectNameEng = objectData.objectNameEng;
                objectSO.description = objectData.description;

                // 열거형 변환
                if (System.Enum.TryParse(objectData.objectTypeString, out ObjectType parsedType))
                {
                    objectSO.objectType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"아이템 '{objectData.objectName}'의 유효하지 않은 타입 : {objectData.objectTypeString}");
                }

                objectSO.stageAppearanceeffect = objectData.stageAppearanceeffect;
                objectSO.StatusEffect = objectData.StatusEffect;

                // 스크립터블 오브젝트 저장 - ID를 4자리 수자로 포맷팅
                string assetPath = $"{outputFolder}/Object_{objectData.id.ToString("D4")}_{objectData.objectNameEng}.asset";
                AssetDatabase.CreateAsset(objectSO, assetPath);

                // 에셋 이름 지정
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
            Debug.LogError($"JSON 변환 오류: . {e}");
        }
    }

    private void ConverterJsonToAchievementScriptableObjects()
    {
        if (!Directory.Exists(outputFolder))                                     
        {
            Directory.CreateDirectory(outputFolder);
        }

        // JSON 파일 읽기
        string jsonText = File.ReadAllText(jsonFilePath);                          

        try
        {
            List<AchievementData> achievementDataList = JsonConvert.DeserializeObject<List<AchievementData>>(jsonText);

            List<AchievementSO> createdAchievements = new List<AchievementSO>();             

            foreach (var achievementData in achievementDataList)
            {
                AchievementSO achievementSO = ScriptableObject.CreateInstance<AchievementSO>();

                // 데이터 복사
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

            // 데이터베이스 생성
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
            Debug.LogError($"JSON 변환 오류: . {e}");
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
                        Debug.LogWarning($"아이템 '{skinData.skinName}'의 아이콘을 찾을 수 없습니다. : {skinData.skinPath}");
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
            Debug.LogError($"JSON 변환 오류: . {e}");
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
                        Debug.LogWarning($"아이템 '{titleData.titleName}'의 아이콘을 찾을 수 없습니다. : {titleData.titlePath}");
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
            Debug.LogError($"JSON 변환 오류: . {e}");
        }
    }
}

#endif