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

        // 무적 상태일 경우 사망 처리 무시
        if (controller != null && controller.IsInvincible)
        {
            Debug.Log("무적 상태이므로 사망하지 않음");
            return;
        }

        isDead = true;

        SoundManager.instance.PlaySound("Die");

        // 즉시 리스폰 처리
        Respawn();
    }

    private void Respawn()
    {
        isDead = false;
        playerRespawn.RespawnPlayer();
    }
}
