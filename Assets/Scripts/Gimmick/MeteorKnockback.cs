using UnityEngine;

public class MeteorKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float upForce = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �˹� ���� (������ �÷��̾� ����)
                Vector3 direction = collision.transform.position - transform.position;
                direction.y = 0f; // ���� �������θ�
                direction.Normalize();
                Vector3 force = direction * knockbackForce + Vector3.up * upForce;
                playerRb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}