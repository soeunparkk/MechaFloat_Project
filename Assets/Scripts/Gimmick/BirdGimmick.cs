
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGimmick : MonoBehaviour, ICheckTrigger // ���� �������̽� ����
{
    private PlayerDie playerDie;

    // --- �߰��� ���� ���� ---
    [Header("�߰��� ����")]
    [Tooltip("�÷��̾ �о�� ���� �����Դϴ�.")]
    public float knockbackForce = 10f;
    [Tooltip("�˹� �� �÷��̾ �ױ������ ��� �ð� (��)�Դϴ�.")]
    public float delayBeforeDeath = 1.5f;
    // -------------------------

    private bool playerAlreadyHitThisSession = false; // �� ��Ϳ� ���� �̹� ���ǿ� ó���Ǿ����� (�ߺ� ������)

    private void Start()
    {
        playerDie = FindObjectOfType<PlayerDie>();
        if (playerDie == null)
        {
            Debug.LogError("BirdGimmick: PlayerDie ��ũ��Ʈ�� ã�� �� �����ϴ�!", gameObject);
        }
    }

    public void OnTriggerEntered(Collider other) // ���� �Լ� �ñ״�ó ����
    {
        // ���� ����: �÷��̾� �±��̰�, playerDie ������ ��ȿ�� ��
        if (other.CompareTag("Player") && playerDie != null)
        {
            // --- �߰��� ���� ���� ---
            if (playerAlreadyHitThisSession) // �̹� �� ���� ���� ó���Ǿ��ٸ� �� �̻� ���� �� ��
            {
                return;
            }
            playerAlreadyHitThisSession = true; // ó�������� ǥ��

            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �˹� ����: ���� ��ġ���� �÷��̾� ������ (�����ϰ�)
                // ���� ��¦ ���� ȿ�� �߰�
                Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
                knockbackDir = (knockbackDir + Vector3.up * 0.5f).normalized; // Y�� �� 0.5 (���� ����)

                playerRb.velocity = Vector3.zero; // ���� �ӵ� �ʱ�ȭ (������)
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse); // �˹�!
            }
            else
            {
                Debug.LogWarning("BirdGimmick: �÷��̾ Rigidbody�� ���� �˹��� ������ �� �����ϴ�.", other.gameObject);
            }

            // ������ �� Die() �Լ� ȣ���ϴ� �ڷ�ƾ ����
            StartCoroutine(DelayedDeathRoutine());
            // --- �߰��� ���� �� ---

            // playerDie.Die(); // << ������ ��� ��� �ڵ�� �ּ� ó���ϰų� ����
        }
    }

    // --- �߰��� �ڷ�ƾ ---
    IEnumerator DelayedDeathRoutine()
    {
        yield return new WaitForSeconds(delayBeforeDeath); // ������ �ð���ŭ ���

        if (playerDie != null) // playerDie�� ������ ��ȿ���� Ȯ��
        {
            playerDie.Die(); // ��� ó��
        }
    }
    // --------------------

    // ���� �� ����� ����Ǿ�� �Ѵٸ� (��: ���� ���µ� ��)
    // playerAlreadyHitThisSession �÷��׸� false�� �ǵ����� �Լ��� �ʿ��� �� �ֽ��ϴ�.
    // public void ResetBirdGimmick()
    // {
    //     playerAlreadyHitThisSession = false;
    // }
}