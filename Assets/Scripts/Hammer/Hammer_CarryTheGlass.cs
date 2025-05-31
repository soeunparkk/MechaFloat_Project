using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer_CarryTheGlass : MonoBehaviour
{
    [Header("Movement Settings - ������ ����")]
    public float swingAngleMax = 45f;
    public float swingSpeed = 100f;
    public float pauseAtEndpoints = 0.5f;

    [Header("Interaction Settings - ��ȣ�ۿ� ����")]
    public float playerKnockbackForce = 5f;
    public Vector2 knockbackDirectionBias = Vector2.up; // �˹� �� ���� ��¦ ���� ���� ����ġ
    public float knockbackBiasStrength = 0.2f; // �˹� ���� ����ġ�� ���� (0~1)

    // ��ġ�� ���� "Balloon" �±װ� ���� ������Ʈ�� �浹���� ��
    public bool destroyBalloonOnDirectHit = true; // ǳ�� ���� Ÿ�� �� �ı� ����
    public float directBalloonHitForce = 1f;      // ǳ�� ���� Ÿ�� �� �о�� �� (������)

    private bool swingingToEnd = true;
    private float currentTargetAngle;
    private float waitTimer = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        currentTargetAngle = GetAngleFromInitial(-swingAngleMax);
        // ���� �� �ʱ� ���� ��� ���� (������)
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTargetAngle);
    }

    float GetAngleFromInitial(float offset)
    {
        return initialRotation.eulerAngles.z + offset;
    }

    void Update()
    {
        if (swingingToEnd)
        {
            currentTargetAngle = GetAngleFromInitial(swingAngleMax);
        }
        else
        {
            currentTargetAngle = GetAngleFromInitial(-swingAngleMax);
        }

        float currentZ = transform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, currentTargetAngle, swingSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newZ);

        if (Mathf.Abs(Mathf.DeltaAngle(newZ, currentTargetAngle)) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= pauseAtEndpoints)
            {
                swingingToEnd = !swingingToEnd;
                waitTimer = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. �÷��̾�� �浹 ��
        if (other.CompareTag("Player"))
        {
            HandlePlayerInteraction(other.gameObject);
        }
        // 2. "Balloon" �±׸� ���� ������Ʈ�� ���� �浹 �� (�÷��̾ ��� �ֵ� �ƴϵ�)
        else if (other.CompareTag("Balloon"))
        {
            HandleDirectBalloonHit(other.gameObject);
        }
    }

    void HandlePlayerInteraction(GameObject playerObject)
    {
        Rigidbody2D playerRb = playerObject.GetComponent<Rigidbody2D>();

        // �÷��̾� �˹�
        if (playerRb != null)
        {
            Vector2 directionToPlayer = (playerObject.transform.position - transform.position).normalized;
            // �������� �ణ�� ���̾�� �־� ���� ���� �߰�
            Vector2 finalKnockbackDir = (directionToPlayer + knockbackDirectionBias.normalized * knockbackBiasStrength).normalized;

            playerRb.velocity = Vector2.zero; // ���� �ӵ� ����
            playerRb.AddForce(finalKnockbackDir * playerKnockbackForce, ForceMode2D.Impulse);
            Debug.Log("Player hit by hammer. Applying knockback.");
        }

        // �÷��̾ ǳ���� ������ �ִ��� Ȯ�� (�ڽ� ������Ʈ �� "Balloon" �±� Ž��)
        // �� ����� �÷��̾��� �ڽ����� ǳ���� �ְ�, �ش� ǳ���� "Balloon" �±װ� �ִٰ� �����մϴ�.
        Transform balloonTransform = FindBalloonInChildren(playerObject.transform);

        if (balloonTransform != null)
        {
            Debug.Log("Player was holding a balloon. Deactivating/Destroying balloon.");
            // ǳ�� ������Ʈ�� ��Ȱ��ȭ�ϰų� �ı��մϴ�.
            // Destroy(balloonTransform.gameObject); // ��� �ı�
            balloonTransform.gameObject.SetActive(false); // ��Ȱ��ȭ (���߿� �ٽ� Ȱ��ȭ ����)

            // ���⿡ �÷��̾ ǳ���� �Ҿ��� ���� �߰����� ������ ���� �� �ֽ��ϴ�.
            // (��: �÷��̾� �̵� �ӵ� ����, Ư�� �ɷ� ��� �� - �÷��̾� ��ũ��Ʈ���� ó���ؾ� ��)
            // �� ��ũ��Ʈ �ܵ����δ� �÷��̾� ���� ���� ������ ��ƽ��ϴ�.
        }
    }

    void HandleDirectBalloonHit(GameObject balloonObject)
    {
        Debug.Log("Hammer directly hit a Balloon object.");

        if (destroyBalloonOnDirectHit)
        {
            // Destroy(balloonObject); // ��� �ı�
            balloonObject.SetActive(false); // ��Ȱ��ȭ
            Debug.Log("Balloon deactivated/destroyed on direct hit.");
        }
        else
        {
            // ���������� �о�� (������)
            Rigidbody2D balloonRb = balloonObject.GetComponent<Rigidbody2D>();
            if (balloonRb != null)
            {
                Vector2 directionToBalloon = (balloonObject.transform.position - transform.position).normalized;
                balloonRb.velocity = Vector2.zero;
                balloonRb.AddForce(directionToBalloon * directBalloonHitForce, ForceMode2D.Impulse);
                Debug.Log("Balloon pushed by direct hit.");
            }
        }
    }

    // �ڽ� ������Ʈ �߿��� "Balloon" �±׸� ���� ù ��° ������Ʈ�� ã�� �Լ�
    Transform FindBalloonInChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Balloon"))
            {
                return child; // ù ��°�� ã�� ǳ�� ��ȯ
            }
            // ��������� �� ���� �ڽĵ� Ž���Ϸ��� �Ʒ� �ּ� ����
            // Transform found = FindBalloonInChildren(child);
            // if (found != null) return found;
        }
        return null; // ǳ���� ã�� ����
    }
}