using UnityEngine;
using TMPro;

public class AchievementPopupUI : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI achievementText;

    public void ShowPopup(AchievementSO achievement)
    {
        popupPanel.SetActive(true);
        achievementText.text = $"업적 달성! : {achievement.achievementName}";

        // 3초 후 자동 숨김
        Invoke(nameof(HidePopup), 3f);
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}
