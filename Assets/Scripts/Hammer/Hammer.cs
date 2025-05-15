using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, ICheckTrigger
{
    public float knockbackForce = 10f;
    public float downAngle = 90f;       // 내려칠 각도 (Z축 기준)
    public float upAngle = 0f;          // 원래 위치 각도
    public float downSpeed = 300f;      // 내려치는 속도 (도/초)
    public float upSpeed = 100f;        // 올라가는 속도
    public float waitTime = 1f;         // 내려친 후 기다리는 시간

    private bool isFalling = true;
    private float waitTimer = 0f;

    void Update()
    {
        float targetAngle = isFalling ? downAngle : upAngle;
        float speed = isFalling ? downSpeed : upSpeed;

        float currentZ = transform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, targetAngle, speed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newZ);

        if (Mathf.Abs(Mathf.DeltaAngle(newZ, targetAngle)) < 0.1f)
        {
            if (isFalling)
            {
                // 내려친 상태일 때 일정 시간 대기
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    isFalling = false;
                    waitTimer = 0f;
                }
            }
            else
            {
                // 다시 내려치기
                isFalling = true;
            }
        }
    }

    public void OnTriggerEntered(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // 플레이어 방향 계산해서 날리기
                Vector3 direction = (other.transform.position - transform.position).normalized;
                rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}
