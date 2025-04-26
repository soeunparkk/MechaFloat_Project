using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishTileWithEffect : MonoBehaviour
{
    public GameObject breakEffectPrefab; // 터질 때 나올 이펙트 프리팹
    public float vanishDelay = 0.5f;      // 밟고 몇 초 후 사라질지
    public float respawnDelay = 0.5f;     // 사라지고 몇 초 후 다시 켜질지

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
        // 이펙트 생성
        if (breakEffectPrefab != null)
        {
            Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(vanishDelay);

        // 타일 끄기
        tileCollider.enabled = false;
        tileRenderer.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // 타일 다시 켜기
        tileCollider.enabled = true;
        tileRenderer.enabled = true;

        isTriggered = false; // 다시 밟을 수 있게 초기화
    }
}
