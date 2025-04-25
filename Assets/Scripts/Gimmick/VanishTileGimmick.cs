using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishTileGimmick : MonoBehaviour
{
    public float vanishDelay = 5.0f;      // 밟고 나서 사라지는 시간
    public float reappearDelay = 3.0f;    // 다시 나타나는 시간

    private bool isTriggered = false;

    private Renderer gimmick_Rend;
    private Collider gimmick_Col;

    void Awake()
    {
        gimmick_Rend = GetComponent<Renderer>();
        gimmick_Col = GetComponent<Collider>();
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

        gimmick_Rend.enabled = false;
        gimmick_Col.enabled = false;

        yield return new WaitForSeconds(reappearDelay);

        gimmick_Rend.enabled = true;
        gimmick_Col.enabled = true;

        isTriggered = false;
    }
}
