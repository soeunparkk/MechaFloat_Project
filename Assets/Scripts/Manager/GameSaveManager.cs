using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager instance;

    public static GameSaveManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return null;
#endif
            if (instance == null)
            {
                Instantiate(Resources.Load<GameSaveManager>("GameSaveManager"));
            }

            return instance;
        }
    }

    public PlayerController Player { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 씬 로드 이벤트 연결
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 씬이 로드될 때마다 자동으로 플레이어를 찾고 Load 호출
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 로드된 씬에서 플레이어 다시 찾기
        PlayerController foundPlayer = GameObject.FindObjectOfType<PlayerController>();
        if (foundPlayer != null)
        {
            Player = foundPlayer;
            SaveSystem.Load(); // 자동 위치 복원
            Debug.Log("[GameSaveManager] 자동 위치 로드 완료");
        }
    }

    // 수동 테스트용
    private void Update()
    {
        SaveSystem.Save();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SaveSystem.Load();
            Debug.Log("[GameSaveManager] 수동 로드 실행됨");
        }
    }
}
