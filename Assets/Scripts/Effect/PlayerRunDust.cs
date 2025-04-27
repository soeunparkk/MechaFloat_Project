using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunDust : MonoBehaviour
{
    public ParticleSystem dustEffectPrefab;
    public Transform footPosition; // �� �ؿ� ���� ��ġ
    private ParticleSystem dustEffectInstance;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (dustEffectPrefab != null && footPosition != null)
        {
            dustEffectInstance = Instantiate(dustEffectPrefab, footPosition.position, Quaternion.identity, footPosition);
            dustEffectInstance.Stop();
        }
        else
        {
            Debug.LogError("DustEffectPrefab �Ǵ� FootPosition ������ �� �ƽ��ϴ�.");
        }
    }

    void Update()
    {
        if (dustEffectInstance == null) return;

        if (isGrounded && rb.velocity.magnitude > 1.0f)
        {
            if (!dustEffectInstance.isPlaying)
                dustEffectInstance.Play();
        }
        else
        {
            if (dustEffectInstance.isPlaying)
                dustEffectInstance.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
