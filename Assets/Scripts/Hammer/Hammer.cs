using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, ICheckTrigger
{
    public float knockbackForce = 10f;
    public float downAngle = 90f;       // ����ĥ ���� (Z�� ����)
    public float upAngle = 0f;          // ���� ��ġ ����
    public float downSpeed = 300f;      // ����ġ�� �ӵ� (��/��)
    public float upSpeed = 100f;        // �ö󰡴� �ӵ�
    public float waitTime = 1f;         // ����ģ �� ��ٸ��� �ð�

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
                // ����ģ ������ �� ���� �ð� ���
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    isFalling = false;
                    waitTimer = 0f;
                }
            }
            else
            {
                // �ٽ� ����ġ��
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
                // �÷��̾� ���� ����ؼ� ������
                Vector3 direction = (other.transform.position - transform.position).normalized;
                rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}
