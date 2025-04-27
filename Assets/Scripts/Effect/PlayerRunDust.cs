using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunDust : MonoBehaviour
{
    public ParticleSystem dustEffectPrefab;
    public Transform footPosition; // 발 밑에 붙일 위치
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
            Debug.LogError("DustEffectPrefab 또는 FootPosition 연결이 안 됐습니다.");
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
