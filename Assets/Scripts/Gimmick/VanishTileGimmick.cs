using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishTileGimmick : MonoBehaviour
{
    public float vanishDelay = 5.0f;
    public float reappearDelay = 3.0f;

    private bool isTriggered = false;

    private Renderer gimmick_Rend;
    public Collider[] gimmick_Colliders;

    private void Start()
    {
        gimmick_Rend = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isTriggered && collision.transform.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(VanishAndReappear());
        }
    }

    private IEnumerator VanishAndReappear()
    {
        yield return new WaitForSeconds(vanishDelay);

        // 1. ���� ���־� �����ݴϴ�.
        gimmick_Rend.enabled = false;

        // 2. �浹 ó���� �ణ ���缭 �÷��̾ ������ �� �ִ� ������ �ݴϴ�.
        yield return new WaitForSeconds(0.2f); // 0.2�� ���� ����

        foreach (var collider in gimmick_Colliders)
        {
            collider.enabled = false;
        }

        yield return new WaitForSeconds(reappearDelay);

        gimmick_Rend.enabled = true;

        foreach (var collider in gimmick_Colliders)
        {
            collider.enabled = true;
        }

        isTriggered = false;
    }
}

