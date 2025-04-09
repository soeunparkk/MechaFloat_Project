using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private Vector3 savedPosition;
    private bool hasSavedPosition = false;

    private Vector3 startPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetStartPosition(Vector3.zero); // 필요시 외부에서 설정 가능
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
    }

    public void SavePlayerPosition(Vector3 position)
    {
        savedPosition = position;
        hasSavedPosition = true;
    }

    public Vector3 GetRespawnPosition()
    {
        return hasSavedPosition ? savedPosition : startPosition;
    }
}
