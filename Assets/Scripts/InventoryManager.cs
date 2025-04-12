using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Hotbar Settings")]
    [SerializeField] private Transform[] hotbarSlots = new Transform[4];

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

        Transform slot = hotbarSlots[slotIndex];
        BalloonController balloon = slotItems[slotIndex];

        // ������ �� ������ ������Ʈ
        slot.Find("Icon").GetComponent<Image>().sprite = balloon?.balloonData.icon;
        Slider durabilitySlider = slot.Find("DurabilityBar").GetComponent<Slider>();

        if (balloon != null)
        {
            durabilitySlider.gameObject.SetActive(true);
            durabilitySlider.maxValue = balloon.balloonData.maxHP;
            durabilitySlider.value = balloon.currentHP;
        }
        else
        {
            durabilitySlider.gameObject.SetActive(false);
        }
    }

    public void UpdateAllSlots()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
            UpdateHotbarUI(i);
    }

    public void RemoveFromInventory(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotItems.Length) return;

        if (slotItems[slotIndex] != null)
        {
            slotItems[slotIndex].OnDurabilityChanged = null; // �̺�Ʈ ���� ����
            slotItems[slotIndex] = null;
        }
        UpdateHotbarUI(slotIndex);
    }

    private void HandleSlotSelection()
    {
        // ���� Ű �Է� ó��
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlotIndex = i;
                UpdateAllSlots();
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

    public bool AddToInventory(BalloonController newBalloon)
    {
        for (int i = 0; i < slotItems.Length; i++)
        {
            if (slotItems[i] == null)
            {
                slotItems[i] = newBalloon;
                newBalloon.assignedSlot = i; // �ű� �߰�: ���� �ε��� ����
                newBalloon.OnDurabilityChanged += () => UpdateHotbarUI(i);
                UpdateHotbarUI(i);
                return true;
            }
        }
        Debug.LogWarning("�κ��丮�� ���� á���ϴ�!");
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