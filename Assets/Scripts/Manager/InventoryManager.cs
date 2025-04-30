using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Hotbar Settings")]
    [SerializeField] private Transform[] hotbarSlots = new Transform[4];
    [SerializeField] private Image[] slotIcons = new Image[4];
    [SerializeField] private Slider durabilitySlider;
    [SerializeField] private Image balloonIcon;

    private int selectedSlotIndex = 0;
    private BalloonController[] slotItems = new BalloonController[4];

    public BalloonController GetSelectedBalloon() => slotItems[selectedSlotIndex];

    private void Awake() => Instance = this;


    private void Update()
    {
        HandleSlotSelection();
    }

    public void UpdateHotbarUI(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= hotbarSlots.Length) return;

        BalloonController balloon = slotItems[slotIndex];

        // 상단 UI는 장착된 경우만
        if (balloon != null && IsBalloonEquipped(slotIndex))
        {
            durabilitySlider.gameObject.SetActive(true);
            balloonIcon.gameObject.SetActive(true);

            durabilitySlider.maxValue = balloon.balloonData.maxHP;
            durabilitySlider.value = balloon.currentHP;
            balloonIcon.sprite = balloon.balloonData.icon;
        }
        else
        {
            durabilitySlider.gameObject.SetActive(false);
            balloonIcon.gameObject.SetActive(false);
        }
    }

    public void RemoveFromInventory(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotItems.Length) return;

        if (slotItems[slotIndex] != null)
        {
            slotItems[slotIndex].OnDurabilityChanged = null; // 이벤트 구독 해제
            slotItems[slotIndex] = null;
        }
        UpdateHotbarUI(slotIndex);
    }

    private void HandleSlotSelection()
    {
        // 숫자 키 입력 처리
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlotIndex = i;
            }
        }
    }

    public void UnequipCurrentBalloon()
    {
        BalloonController currentEquipped = GetEquippedBalloon();
        if (currentEquipped != null)
        {
            currentEquipped.transform.SetParent(transform);
            currentEquipped.gameObject.SetActive(false);
            currentEquipped.StopDurabilityReduction();
            UpdateHotbarUI(currentEquipped.assignedSlot);
        }
    }

    public void UpdateEquippedBalloonUI()
    {
        BalloonController equipped = GetEquippedBalloon();
        if (equipped != null)
        {
            durabilitySlider.gameObject.SetActive(true);
            balloonIcon.gameObject.SetActive(true);
            durabilitySlider.maxValue = equipped.balloonData.maxHP;
            durabilitySlider.value = equipped.currentHP;
            balloonIcon.sprite = equipped.balloonData.icon;
        }
        else
        {
            durabilitySlider.gameObject.SetActive(false);
            balloonIcon.gameObject.SetActive(false);
        }
    }

    public bool AddToInventory(BalloonController newBalloon)
    {
        for (int i = 0; i < slotItems.Length; i++)
        {
            if (slotItems[i] == null)
            {
                slotItems[i] = newBalloon;
                newBalloon.assignedSlot = i;
                newBalloon.OnDurabilityChanged += () => UpdateEquippedBalloonUI();

                if (slotIcons[i] != null)
                {
                    slotIcons[i].sprite = newBalloon.balloonData.icon;
                    slotIcons[i].enabled = true;
                    slotIcons[i].gameObject.SetActive(true);
                }

                return true;
            }
        }

        Debug.LogWarning("인벤토리가 가득 찼습니다!");
        return false;
    }


    public bool IsBalloonEquipped(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotItems.Length) return false;
        return slotItems[slotIndex] != null && slotItems[slotIndex].gameObject.activeSelf;
    }

    public BalloonController GetEquippedBalloon()
    {
        foreach (var balloon in slotItems)
        {
            if (balloon != null && balloon.gameObject.activeSelf)
            {
                return balloon;
            }
        }
        return null;
    }

    public BalloonController GetBalloonAtSlot(int index)
    {
        if (index < 0 || index >= slotItems.Length) return null;
        return slotItems[index];
    }
}