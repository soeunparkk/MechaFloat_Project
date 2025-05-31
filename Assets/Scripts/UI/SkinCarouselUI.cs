using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinCarouselUI : MonoBehaviour
{
    [Header("UI References")]
    public Image skinLeftImage;
    public Image skinCenterImage;
    public Image skinRightImage;
    public TextMeshProUGUI skinNameText;
    public Button leftButton;
    public Button rightButton;

    [Header("Settings")]
    public List<SkinSO> skins;
    private int currentIndex = 0;

    private void Start()
    {
        leftButton.onClick.AddListener(OnClickLeft);
        rightButton.onClick.AddListener(OnClickRight);
        RefreshUI();
    }

    private void OnClickLeft()
    {
        currentIndex = (currentIndex - 1 + skins.Count) % skins.Count;
        RefreshUI();
    }

    private void OnClickRight()
    {
        currentIndex = (currentIndex + 1) % skins.Count;
        RefreshUI();
    }

    private void RefreshUI()
    {
        int leftIndex = (currentIndex - 1 + skins.Count) % skins.Count;
        int rightIndex = (currentIndex + 1) % skins.Count;

        skinLeftImage.sprite = skins[leftIndex].skinImage;
        skinCenterImage.sprite = skins[currentIndex].skinImage;
        skinRightImage.sprite = skins[rightIndex].skinImage;

        skinNameText.text = skins[currentIndex].skinName;

        // 크기 조절 (강조 효과)
        skinLeftImage.rectTransform.localScale = Vector3.one * 0.7f;
        skinCenterImage.rectTransform.localScale = Vector3.one;
        skinRightImage.rectTransform.localScale = Vector3.one * 0.7f;
    }
}
