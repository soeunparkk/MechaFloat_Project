using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform balloonPivot;
    public float pickupRange = 1.0f;

    private GameObject currentBalloon = null;
    private GameObject nearbyBalloon = null;

    void Update()
    {
        FindNearbyBalloon();
    }

    public void HandlePickup()
    {
        if (currentBalloon == null && nearbyBalloon != null)
        {
            PickupBalloon();
        }
        else if (currentBalloon != null)
        {
            DropBalloon();
        }
    }

    private void FindNearbyBalloon()
    {
        Collider[] balloonCol = Physics.OverlapSphere(transform.position, pickupRange);
        nearbyBalloon = null;

        foreach (Collider colliders in balloonCol)
        {
            if (colliders.CompareTag("Balloon"))
            {
                nearbyBalloon = colliders.gameObject;
                break;
            }
        }
    }

    public ItemSO GetCurrentBalloonData()
    {
        if (currentBalloon != null)
        {
            BalloonController balloon = currentBalloon.GetComponent<BalloonController>();
            return balloon != null ? balloon.balloonData : null;
        }
        return null;
    }

    private void PickupBalloon()
    {
        if (nearbyBalloon == null) return;

        currentBalloon = nearbyBalloon;
        currentBalloon.transform.SetParent(balloonPivot);
        currentBalloon.transform.localPosition = Vector3.zero;
        currentBalloon.transform.localRotation = Quaternion.identity;

        Rigidbody rb = currentBalloon.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        currentBalloon.GetComponent<BalloonController>()?.StartDurabilityReduction();

        GetComponent<PlayerController>().PickupBalloon();
        nearbyBalloon = null;
    }


    // 드랍을 할때 원래 스폰했던 풍선 위치로 드랍되는건지 아니면 자기 앞에 그냥 떨어지는건지 알면 수정 예정
    private void DropBalloon()
    {
        if (currentBalloon == null) return;

        currentBalloon.transform.SetParent(null);

        Rigidbody rb = currentBalloon.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        currentBalloon.transform.position += Vector3.down * 0.5f;

        currentBalloon.GetComponent<BalloonController>()?.StopDurabilityReduction();

        currentBalloon = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
