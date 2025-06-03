using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float speed = 15f; // 운석 속도

    Transform player;     
    Transform safeZone;   

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        GameObject s = GameObject.Find("SafeZone");
        if (s != null) safeZone = s.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        transform.LookAt(player);
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어랑 부딪히면
        if (other.CompareTag("Player") && safeZone != null)
        {
            // 세이프존으로 순간이동
            other.transform.position = safeZone.position;
        }
    }
}
