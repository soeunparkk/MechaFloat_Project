using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    private PlayerController controller;

    [SerializeField] private PlayerRespawn playerRespawn;

    private bool isDead = false;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void Die()
    {
        if (isDead) return;

        // ���� ������ ��� ��� ó�� ����
        if (controller != null && controller.IsInvincible)
        {
            Debug.Log("���� �����̹Ƿ� ������� ����");
            return;
        }

        isDead = true;

        SoundManager.instance.PlaySound("Die");

        // ��� ������ ó��
        Respawn();
    }

    private void Respawn()
    {
        isDead = false;
        playerRespawn.RespawnPlayer();
    }
}
