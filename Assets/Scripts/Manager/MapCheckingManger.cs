using UnityEngine;
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

    public void SetCurrentStage(StageSO newStageSO)
    {
        if (newStageSO == null) return;

        currentStageSO = newStageSO;
        Debug.Log($"스테이지 변경됨: {currentStageSO.stageName}");

        PlayerJump playerJump = FindObjectOfType<PlayerJump>();
        if (playerJump != null)
        {
            playerJump.SetGravityState(currentStageSO.isZeroGravityMap);
        }
        else
        {
            Debug.LogWarning("PlayerJump를 찾을 수 없습니다!");
        }
    }

}
