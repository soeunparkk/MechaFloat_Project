using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWindZone : MonoBehaviour
{
    [Header("초기 상승 설정")]
    [Tooltip("바람 영역 진입 시 초기 수직 상승 힘의 강도입니다.")]
    public float initialUpwardBoostForce = 50f;
    [Tooltip("초기 상승 힘을 적용할 방식입니다.")]
    public ForceMode initialBoostForceMode = ForceMode.Impulse; // 순간적인 힘

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
    public float playerMultiplier = 1.0f;
    public float playerWithBalloonMultiplier = 1.1f;
    public float balloonMultiplier = 1.2f;

    [Header("Drag 설정")]
    public float dragInWind = 1.0f; // 부유 효과를 위해 드래그를 조금 높여 안정시킬 수 있음
    public float normalDrag = 0.3f;

    // 내부 관리용 클래스 및 리스트
    private class AffectedBody
    {
        public Rigidbody rb;
        public float originalDrag;
        public bool originalUseGravity;
        public float timeInZone; // 영역에 머문 시간 (부유 주기 계산용)
        public bool initialBoostApplied; // 초기 부스트 적용 여부

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
        Vector3 windDirection = transform.up; // 바람은 계속 위로

        for (int i = bodiesInZone.Count - 1; i >= 0; i--) // 역순 순회 (제거 시 안전)
        {
            AffectedBody affected = bodiesInZone[i];
            if (affected.rb == null) // Rigidbody가 파괴된 경우
            {
                bodiesInZone.RemoveAt(i);
                continue;
            }

            affected.timeInZone += Time.fixedDeltaTime; // 영역 내 시간 누적

            // 1. 초기 급상승 힘 (한 번만 적용)
            if (!affected.initialBoostApplied)
            {
                float boostMultiplier = GetMultiplier(affected.rb);
                affected.rb.AddForce(windDirection * initialUpwardBoostForce * boostMultiplier, initialBoostForceMode);
                affected.initialBoostApplied = true;
            }

            // 2. 지속적인 부유 힘 (사인파를 이용한 변화)
            float hoverFactor = 0f;
            if (hoverCycleDuration > 0)
            {
                // 시간 경과에 따라 -1과 1 사이를 반복하는 사인 값
                hoverFactor = Mathf.Sin((affected.timeInZone / hoverCycleDuration) * 2 * Mathf.PI);
            }

            // 기본 상승 힘에 부유 변화량을 더함
            float currentSustainedForce = sustainedUpwardForce + (hoverFactor * hoverForceAmplitude);
            float forceMultiplier = GetMultiplier(affected.rb);

            affected.rb.AddForce(windDirection * currentSustainedForce * forceMultiplier, ForceMode.Force);
        }
    }

    // 오브젝트 타입에 따른 힘 배율 반환 함수
    private float GetMultiplier(Rigidbody rb)
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
        return 1.0f; // 기본값
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.8f, 1f, 0.7f); // 약간 밝은 하늘색

        float gizmoLineLength = 5f;
        float arrowHeadLength = 0.75f;
        float arrowHeadAngle = 20.0f;

        Vector3 direction = transform.up;
        Vector3 endPoint = transform.position + direction * gizmoLineLength;

        Gizmos.DrawLine(transform.position, endPoint);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
        Gizmos.DrawLine(endPoint, endPoint + right * arrowHeadLength);
        Gizmos.DrawLine(endPoint, endPoint + left * arrowHeadLength);

        // 부유 범위 시각화 (선택 사항)
        if (Application.isPlaying && bodiesInZone.Count > 0) // 에디터 실행 중에만, 안에 뭔가 있을 때
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // 주황색 반투명
            float avgAltitude = transform.position.y + 3f; // 대략적인 평균 고도 (예시)
            float amplitudeViz = hoverForceAmplitude * 0.1f; // 힘의 크기를 시각적 변화로 변환 (스케일 조정 필요)
            Gizmos.DrawLine(transform.position + Vector3.up * (gizmoLineLength - amplitudeViz), transform.position + Vector3.up * (gizmoLineLength + amplitudeViz));
        }
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