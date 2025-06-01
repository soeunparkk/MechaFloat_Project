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
    public GameObject lockIconLeft, lockIconCenter, lockIconRight;
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

        UpdateSkinSlot(skinLeftImage, lockIconLeft, skins[leftIndex]);
        UpdateSkinSlot(skinCenterImage, lockIconCenter, skins[currentIndex]);
        UpdateSkinSlot(skinRightImage, lockIconRight, skins[rightIndex]);

        skinNameText.text = skins[currentIndex].skinName;
    }

    private void UpdateSkinSlot(Image image, GameObject lockIcon, SkinSO skin)
    {
        image.sprite = skin.skinImage;

        bool isUnlocked = skin.unlockCondition == null || skin.unlockCondition.isAchievement;
        lockIcon.SetActive(!isUnlocked);

        image.color = isUnlocked ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
    }
}
