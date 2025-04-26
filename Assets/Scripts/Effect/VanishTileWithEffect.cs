using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishTileWithEffect : MonoBehaviour
{
    public GameObject breakEffectPrefab; // ���� �� ���� ����Ʈ ������
    public float vanishDelay = 0.5f;      // ��� �� �� �� �������
    public float respawnDelay = 0.5f;     // ������� �� �� �� �ٽ� ������

    private Collider tileCollider;
    private Renderer tileRenderer;

    private bool isTriggered = false;

    void Start()
    {
        tileCollider = GetComponent<Collider>();
        tileRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(VanishAndRespawn());
        }
    }

    private IEnumerator VanishAndRespawn()
    {
        // ����Ʈ ����
        if (breakEffectPrefab != null)
        {
            Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(vanishDelay);

        // Ÿ�� ����
        tileCollider.enabled = false;
        tileRenderer.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Ÿ�� �ٽ� �ѱ�
        tileCollider.enabled = true;
        tileRenderer.enabled = true;

        isTriggered = false; // �ٽ� ���� �� �ְ� �ʱ�ȭ
    }
}
