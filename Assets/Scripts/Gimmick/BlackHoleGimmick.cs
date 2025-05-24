using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGimmick : MonoBehaviour, ICheckTrigger
{
    #region ���۷���
    [Header("������ ��� �±�")]
    [SerializeField] private string targetTag = "Player";

    [Header("������� ��")]
    [SerializeField] private float pullForce = 5f;

    [Header("�߾� ����Ʈ")]
    [SerializeField] private Transform centerPoint;

    [Header("���� ��ġ")]
    [SerializeField] private Transform spawnPoint;

    [Header("���� ����")]
    [SerializeField] private Collider pullTriggerArea;

    // ���ο��� ������ ��ü ����Ʈ
    private readonly List<Rigidbody> affectedBodies = new List<Rigidbody>();

    #endregion

    #region �ʱ�ȭ
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

    #region ��Ȧ Ʈ����
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

    #region ����� ������
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (pullTriggerArea == null) return;

        Gizmos.color = new Color(1f, 0.2f, 0.6f, 0.3f);

        // SphereCollider �ð�ȭ
        if (pullTriggerArea is SphereCollider sphere)
        {
            Vector3 center = sphere.transform.TransformPoint(sphere.center);
            float radius = sphere.radius * Mathf.Max(
                sphere.transform.lossyScale.x,
                sphere.transform.lossyScale.y,
                sphere.transform.lossyScale.z);

            Gizmos.DrawSphere(center, radius);
        }

        // BoxCollider �ð�ȭ
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
