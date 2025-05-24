using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGimmick : MonoBehaviour, ICheckTrigger
{
    #region 레퍼런스
    [Header("끌려갈 대상 태그")]
    [SerializeField] private string targetTag = "Player";

    [Header("끌어당기는 힘")]
    [SerializeField] private float pullForce = 5f;

    [Header("중앙 포인트")]
    [SerializeField] private Transform centerPoint;

    [Header("스폰 위치")]
    [SerializeField] private Transform spawnPoint;

    [Header("당기는 영역")]
    [SerializeField] private Collider pullTriggerArea;

    // 내부에서 추적할 객체 리스트
    private readonly List<Rigidbody> affectedBodies = new List<Rigidbody>();

    #endregion

    #region 초기화
    private void FixedUpdate()
    {
        foreach (var body in affectedBodies)
        {
            if (body == null) continue;

            Debug.Log($"[Pulling] {body.name}");

            Vector3 direction = (centerPoint.position - body.position).normalized;
            body.AddForce(direction * pullForce, ForceMode.Acceleration);
        }
    }

    #endregion

    #region 블랙홀 트리거
    public void OnTriggerEntered(Collider other)
    {
        if (other.CompareTag(targetTag) && other.attachedRigidbody != null)
        {
            if (!affectedBodies.Contains(other.attachedRigidbody))
                affectedBodies.Add(other.attachedRigidbody);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        float distance = Vector3.Distance(other.transform.position, centerPoint.position);
        if (distance < 0.5f)
        {
            other.transform.position = spawnPoint.position;

            if (other.attachedRigidbody != null)
            {
                other.attachedRigidbody.velocity = Vector3.zero;
                other.attachedRigidbody.angularVelocity = Vector3.zero;
            }

            affectedBodies.Remove(other.attachedRigidbody);
        }
    }

    #endregion

    #region 기즈모 에디터
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (pullTriggerArea == null) return;

        Gizmos.color = new Color(1f, 0.2f, 0.6f, 0.3f);

        // SphereCollider 시각화
        if (pullTriggerArea is SphereCollider sphere)
        {
            Vector3 center = sphere.transform.TransformPoint(sphere.center);
            float radius = sphere.radius * Mathf.Max(
                sphere.transform.lossyScale.x,
                sphere.transform.lossyScale.y,
                sphere.transform.lossyScale.z);

            Gizmos.DrawSphere(center, radius);
        }

        // BoxCollider 시각화
        if (pullTriggerArea is BoxCollider box)
        {
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = box.transform.localToWorldMatrix;

            Gizmos.DrawCube(box.center, box.size);
            Gizmos.matrix = oldMatrix;
        }
    }
#endif

    #endregion
}
