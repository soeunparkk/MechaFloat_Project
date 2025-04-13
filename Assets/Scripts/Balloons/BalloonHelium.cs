using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonHelium : MonoBehaviour
{
    public Transform target;            // 플레이어 머리 Transform
    public float liftForce = 1.5f;
    public float duration = 10f;

    private Rigidbody targetRb;
    private PlayerController player;
    private bool isActive = false; // 풍선 활성화 여부

    private void FixedUpdate()
    {
        // 🎈 풍선이 활성화되었을 때만 힘을 준다
        if (isActive && targetRb != null)
        {
            targetRb.AddForce(Vector3.up * liftForce, ForceMode.Force);
        }
    }

    public void SetTarget(Transform t)
    {
        target = t;

        // 🎯 플레이어 설정
        player = t.GetComponent<PlayerController>() ?? t.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.PickupBalloon();
        }

        // 🎯 Rigidbody 설정
        targetRb = target.GetComponent<Rigidbody>();
        if (targetRb == null)
        {
            targetRb = target.gameObject.AddComponent<Rigidbody>();
            targetRb.useGravity = false;
        }

        // 🎈 풍선 Rigidbody 제거 (본인 물리 작용 X)
        if (TryGetComponent<Rigidbody>(out var selfRb))
        {
            Destroy(selfRb);
        }

        AttachToTarget();
        Invoke(nameof(DestroyBalloon), duration);

        isActive = true;

        
        player = t.GetComponent<PlayerController>() ?? t.GetComponentInParent<PlayerController>();

        // 🎈 풍선 띄우기 시작
        isActive = true;

        // 🎯 DestroyBalloon 예약
        Invoke(nameof(DestroyBalloon), duration);
    }

    private void AttachToTarget()
    {
        transform.SetParent(target);
        transform.localPosition = new Vector3(0, 1.2f, 0); // 머리 위 고정 위치
    }

    private void DestroyBalloon()
    {
        Debug.Log("💥 헬륨 풍선 터짐!");
        if (player != null)
        {
            player.DropBalloon();
        }

        Destroy(gameObject);
    }


}
