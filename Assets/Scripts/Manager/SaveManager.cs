using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private Vector3 savedPosition;
    private bool hasSavedPosition = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerPosition(Vector3 position)
    {
        savedPosition = position;
        hasSavedPosition = true;
    }

    public bool TryGetSavedPosition(out Vector3 position)
    {
        position = savedPosition;
        return hasSavedPosition;
    }
}
