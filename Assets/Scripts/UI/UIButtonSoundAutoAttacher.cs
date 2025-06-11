#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class UIButtonSoundAutoAttacher
{
    [MenuItem("Tools/UI/Attach UIButtonSoundPlayer To All Buttons")]
    public static void AttachToAllButtons()
    {
        Button[] allButtons = Object.FindObjectsOfType<Button>(true);
        int attachedCount = 0;

        foreach (Button button in allButtons)
        {
            if (!button.TryGetComponent<UIButtonSoundPlayer>(out _))
            {
                Undo.AddComponent<UIButtonSoundPlayer>(button.gameObject);
                attachedCount++;
            }
        }

        Debug.Log($"[UIButtonSoundAutoAttacher] {attachedCount}개의 버튼에 UIButtonSoundPlayer가 추가되었습니다.");
    }
}
#endif
