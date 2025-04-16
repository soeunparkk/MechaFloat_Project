using UnityEngine;
using TMPro;

public class AchievementPopupUI : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI achievementText;

    public void ShowPopup(AchievementSO achievement)
    {
        popupPanel.SetActive(true);
        achievementText.text = $"���� �޼�! : {achievement.achievementName}";

        // 3�� �� �ڵ� ����
        Invoke(nameof(HidePopup), 3f);
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}
