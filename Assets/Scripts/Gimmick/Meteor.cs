using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float forcePower = 1000f;
    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 충돌 감지 세팅 안전하게 보강
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // 수평만

        rb.AddForce(direction * forcePower);
    }

    void FixedUpdate()
    {
        // 속도 제한
        float maxSpeed = 40f;
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Update()
    {
        // Y좌표 너무 낮으면 리셋
        if (transform.position.y < -10f)
        {
            transform.position = new Vector3(0, 5, 0);
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            GameObject safeZone = GameObject.Find("savezon_1");
            if (safeZone != null)
            {
                other.transform.position = safeZone.transform.position;
            }
        }
    }
}
