using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스크립트 파일 이름: CustomWindZone.cs (또는 사용자가 변경한 이름)
public class CustomWindZone : MonoBehaviour
{
    [Header("초기 상승 설정")]
    [Tooltip("바람 영역 진입 시 초기 수직 상승 힘의 강도입니다.")]
    public float initialUpwardBoostForce = 50f;
    [Tooltip("초기 상승 힘을 적용할 방식입니다.")]
    public ForceMode initialBoostForceMode = ForceMode.Impulse;

    [Header("부유 설정")]
    [Tooltip("지속적으로 가해지는 기본 수직 상승 힘의 강도입니다.")]
    public float sustainedUpwardForce = 15f;
    [Tooltip("부유 효과를 위한 힘 변화의 주기 (초)입니다. 낮을수록 빠르게 변합니다.")]
    public float hoverCycleDuration = 2f;
    [Tooltip("부유 효과로 인해 힘이 변하는 범위 (위아래 진폭)입니다.")]
    public float hoverForceAmplitude = 5f;
    [Tooltip("바람 영역 내에서 오브젝트의 중력을 비활성화할지 여부입니다.")]
    public bool disableGravityInZone = true;

    [Header("힘 조절 (오브젝트별 배율)")]
    [Tooltip("풍선 없는 플레이어에게 적용될 힘 배율입니다.")]
    public float playerMultiplier = 1.0f;
    [Tooltip("풍선 있는 플레이어에게 적용될 힘 배율입니다. 이 값을 낮춰 과도한 상승을 줄일 수 있습니다.")]
    public float playerWithBalloonMultiplier = 0.8f;
    [Tooltip("독립적인 풍선 오브젝트에게 적용될 힘 배율입니다.")]
    public float balloonMultiplier = 1.2f;

    [Header("풍선 플레이어 상승 속도 제한 (선택 사항)")]
    [Tooltip("풍선 든 플레이어의 최대 수직 상승 속도를 제한할지 여부입니다.")]
    public bool limitBalloonPlayerMaxSpeed = false;
    [Tooltip("풍선을 든 플레이어의 최대 수직 상승 속도입니다. (limitBalloonPlayerMaxSpeed가 true일 때 적용)")]
    public float maxUpwardSpeedForBalloonPlayer = 8f;

    [Header("Drag 설정")]
    public float dragInWind = 1.0f;
    public float normalDrag = 0.3f;

    // 내부 관리용 클래스 및 리스트
    private class AffectedBody
    {
        public Rigidbody rb;
        public float originalDrag;
        public bool originalUseGravity;
        public float timeInZone;
        public bool initialBoostApplied;

        public AffectedBody(Rigidbody body)
        {
            rb = body;
            originalDrag = body.drag;
            originalUseGravity = body.useGravity;
            timeInZone = 0f;
            initialBoostApplied = false;
        }
    }
    private List<AffectedBody> bodiesInZone = new List<AffectedBody>();

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (bodiesInZone.Find(b => b.rb == rb) == null)
            {
                AffectedBody affectedBody = new AffectedBody(rb);
                bodiesInZone.Add(affectedBody);

                rb.drag = dragInWind;
                if (disableGravityInZone)
                {
                    rb.useGravity = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            AffectedBody affectedBody = bodiesInZone.Find(b => b.rb == rb);
            if (affectedBody != null)
            {
                rb.drag = affectedBody.originalDrag;
                rb.useGravity = affectedBody.originalUseGravity;
                bodiesInZone.Remove(affectedBody);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 windUpDirection = transform.up; // 바람은 계속 위로

        for (int i = bodiesInZone.Count - 1; i >= 0; i--)
        {
            AffectedBody affected = bodiesInZone[i];
            if (affected.rb == null)
            {
                bodiesInZone.RemoveAt(i);
                continue;
            }

            affected.timeInZone += Time.fixedDeltaTime;
            float forceMultiplier = GetForceMultiplier(affected.rb);

            if (!affected.initialBoostApplied)
            {
                affected.rb.AddForce(windUpDirection * initialUpwardBoostForce * forceMultiplier, initialBoostForceMode);
                affected.initialBoostApplied = true;
            }

            float hoverFactor = 0f;
            if (hoverCycleDuration > 0)
            {
                hoverFactor = Mathf.Sin((affected.timeInZone / hoverCycleDuration) * 2 * Mathf.PI);
            }

            float currentSustainedForceValue = sustainedUpwardForce + (hoverFactor * hoverForceAmplitude);
            Vector3 sustainedForceVector = windUpDirection * currentSustainedForceValue * forceMultiplier;

            PlayerController player = affected.rb.GetComponent<PlayerController>();
            bool isPlayerWithBalloon = (player != null && player.HasBalloon);

            if (limitBalloonPlayerMaxSpeed && isPlayerWithBalloon)
            {
                if (sustainedForceVector.y > 0 && affected.rb.velocity.y > maxUpwardSpeedForBalloonPlayer)
                {
                    sustainedForceVector.y = 0;
                }
            }
            affected.rb.AddForce(sustainedForceVector, ForceMode.Force);
        }
    }

    private float GetForceMultiplier(Rigidbody rb)
    {
        PlayerController player = rb.GetComponent<PlayerController>();
        if (player != null)
        {
            return player.HasBalloon ? playerWithBalloonMultiplier : playerMultiplier;
        }
        BalloonController balloon = rb.GetComponent<BalloonController>();
        if (balloon != null)
        {
            return balloonMultiplier;
        }
        return 1.0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // <<< Gizmo 색상을 빨간색으로 변경했습니다!

        float gizmoLineLength = 5f;
        float arrowHeadLength = 0.75f;
        float arrowHeadAngle = 20.0f;

        // 현재 스크립트는 바람 방향을 transform.up (오브젝트의 로컬 Y축, 위쪽)으로 사용하고 있습니다.
        // 따라서 Gizmo도 transform.up을 기준으로 그립니다.
        Vector3 direction = transform.up;
        Vector3 endPoint = transform.position + direction * gizmoLineLength;

        Gizmos.DrawLine(transform.position, endPoint);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
        Gizmos.DrawLine(endPoint, endPoint + right * arrowHeadLength);
        Gizmos.DrawLine(endPoint, endPoint + left * arrowHeadLength);
    }

    private void OnDisable()
    {
        foreach (AffectedBody affected in bodiesInZone)
        {
            if (affected.rb != null)
            {
                affected.rb.drag = affected.originalDrag;
                affected.rb.useGravity = affected.originalUseGravity;
            }
        }
        bodiesInZone.Clear();
    }
}