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

            // �� �ε� �̺�Ʈ ����
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� �ε�� ������ �ڵ����� �÷��̾ ã�� Load ȣ��
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �ε�� ������ �÷��̾� �ٽ� ã��
        PlayerController foundPlayer = GameObject.FindObjectOfType<PlayerController>();
        if (foundPlayer != null)
        {
            Player = foundPlayer;
            SaveSystem.Load(); // �ڵ� ��ġ ����
            Debug.Log("[GameSaveManager] �ڵ� ��ġ �ε� �Ϸ�");
        }
    }

    // ���� �׽�Ʈ��
    private void Update()
    {
        SaveSystem.Save();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SaveSystem.Load();
            Debug.Log("[GameSaveManager] ���� �ε� �����");
        }
    }
}
