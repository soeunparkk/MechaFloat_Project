﻿using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform balloonPivot;
    [SerializeField] private float pickupRange = 2f;

    private void Update()
    {
        AutoPickupBalloon();
    }

    private void AutoPickupBalloon()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Balloon") &&
                hit.TryGetComponent(out BalloonController balloon) &&
                balloon.assignedSlot == -1) // 이미 인벤토리에 있는 풍선 제외
            {
                AttemptAddToInventory(balloon);
            }
        }
    }

    private void AttemptAddToInventory(BalloonController balloon)
    {
        bool success = InventoryManager.Instance.AddToInventory(balloon);
        if (success)
        {
            StoreBalloon(balloon);
        }
    }

    private void StoreBalloon(BalloonController balloon)
    {
        balloon.transform.SetParent(transform);
        balloon.transform.localPosition = Vector3.zero;
        balloon.gameObject.SetActive(false);
    }

    public void HandleEquipmentToggle()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            BalloonController selected = InventoryManager.Instance.GetSelectedBalloon();
            if (selected == null) return;

            if (InventoryManager.Instance.IsBalloonEquipped(selected.assignedSlot))
            {
                UnequipBalloon(selected);
            }
            else
            {
                EquipBalloon(selected);
            }
        }
    }

    private void EquipBalloon(BalloonController balloon)
    {
        InventoryManager.Instance.UnequipCurrentBalloon();

        balloon.transform.SetParent(balloonPivot);
        balloon.transform.localPosition = Vector3.zero;
        balloon.gameObject.SetActive(true);
        balloon.StartDurabilityReduction();
        InventoryManager.Instance.UpdateHotbarUI(balloon.assignedSlot);
    }

    private void UnequipBalloon(BalloonController balloon)
    {
        balloon.transform.SetParent(transform);
        balloon.transform.localPosition = Vector3.zero;
        balloon.gameObject.SetActive(false);
        balloon.StopDurabilityReduction();
        InventoryManager.Instance.UpdateHotbarUI(balloon.assignedSlot);
    }
}