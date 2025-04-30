using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AchievementPopupUI : MonoBehaviour
{
    public GameObject popupPanel;

    [Header("Text")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI achievementTitleText;
    public TextMeshProUGUI skinNameText;
    public TextMeshProUGUI titleNameText;

    [Header("Image")]
    public Image skinImage;

    [Header("Animation")]
    public Transform popupTransform;
    public float hiddenY = 800f;
    public float showY = 500f;
    public float duration = 0.5f;
    public float stayTime = 2.5f;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = popupTransform.localPosition;
    }

    public void ShowPopup(AchievementSO achievement, SkinSO skin, TitleSO title)
    {
        popupPanel.SetActive(true);

        titleText.text = "¾÷Àû ´Þ¼º!";
        achievementTitleText.text = $"'{achievement.achievementName}' - {achievement.description}";
        skinNameText.text = skin != null ? $"½ºÅ² ÇØÁ¦: {skin.skinName}" : "";
        titleNameText.text = title != null ? $"ÄªÈ£È¹µæ: {title.titleName}" : "";
        skinImage.sprite = skin != null ? skin.skinImage : null;
        skinImage.gameObject.SetActive(skin != null);

        float x = popupTransform.localPosition.x;
        float z = popupTransform.localPosition.z;

        Vector3 hiddenPos = new Vector3(x, hiddenY, z);
        Vector3 showPos = new Vector3(x, showY, z);
        popupTransform.localPosition = hiddenPos;

        Sequence seq = DOTween.Sequence();
        seq.Append(popupTransform.DOLocalMoveY(showY, duration).SetEase(Ease.OutBack));
        seq.AppendInterval(stayTime);
        seq.Append(popupTransform.DOLocalMoveY(hiddenY, duration).SetEase(Ease.InBack));
        seq.OnComplete(() => popupPanel.SetActive(false));
    }
}
