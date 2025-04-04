﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MapCheckingManager : MonoBehaviour
{
    public static MapCheckingManager instance;

    public StageSO currentStageSO { get; private set; }
    public StageDatabaseSO stageDatabase;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateCurrentStage();
    }

    public void UpdateCurrentStage()
    {
        if (stageDatabase == null)
        {
            Debug.LogError("StageDatabaseSO가 할당되지 않았습니다!");
            return;
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        currentStageSO = stageDatabase.GetItemByName(currentSceneName);

        if (currentStageSO != null)
        {
            Debug.Log($"현재 스테이지: {currentStageSO.stageName} (씬: {currentSceneName})");
        }
        else
        {
            Debug.LogWarning($"씬 {currentSceneName}에 해당하는 스테이지를 찾을 수 없음!");
        }
    }
}
