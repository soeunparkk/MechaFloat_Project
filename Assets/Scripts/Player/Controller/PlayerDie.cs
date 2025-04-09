using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    [SerializeField] private PlayerRespawn playerRespawn;

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // ��� �� ó�� (�ִϸ��̼�, ����Ʈ �� �߰� ����)
        Debug.Log("�÷��̾� ���");

        // ��� ������ ó��
        Respawn();
    }

    private void Respawn()
    {
        isDead = false;
        playerRespawn.RespawnPlayer();
    }
}
